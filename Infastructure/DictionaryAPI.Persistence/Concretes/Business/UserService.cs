using Azure.Core;
using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Application.Abstracts.Security.Hash;
using DictionaryAPI.Application.Abstracts.Security.JWT;
using DictionaryAPI.Application.Abstracts.Services.EmailService;
using DictionaryAPI.Application.DTO.DTOs;
using DictionaryAPI.Application.DTO.DTOValidators;
using DictionaryAPI.Application.Utils;
using DictionaryAPI.Application.Utils.Constants;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Entities;
using DictionaryAPI.Persistence.Contexts;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Mail;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Persistence.Concretes.Business
{
    public class UserService : IUserService
    {
        IHashHelper _hashHelper;
        IUserDal _userDal;
        IConfiguration _configuration;
        IUtilService _utilService;
        IEmailService _emailService;
        IJwtHelper _jwtHelper;
        DictionaryContext _context;
        public UserService(IHashHelper hashHelper, IUserDal userDal, IConfiguration configuration, IUtilService utilService, IEmailService emailService, IJwtHelper jwtHelper, DictionaryContext context)
        {
            _hashHelper = hashHelper;
            _userDal = userDal;
            _configuration = configuration;
            _utilService = utilService;
            _emailService = emailService;
            _jwtHelper = jwtHelper;
            _context = context;
        }

        public Result SignUp(SignUpDto signUpDto)
        {
            SignUpDtoValidator validator = new(); //Creating an instance from DTO Validator class
            ValidationResult result = validator.Validate(signUpDto); //Validate DTO

            if (result.IsValid == false)
            {
                return new ErrorDataResult<List<ValidationFailure>>(result.Errors); //If DTO is not valid, return errors to client
            }

            //Unique check will be added.

            //Salt & Hash
            Tuple<byte[], byte[]> hashResult = _hashHelper.GenerateHash(signUpDto.Password);

            //Creating User Instance
            User user = new(
                username: signUpDto.Username,
                email: signUpDto.Email,
                birthDate: signUpDto.BirthDate,
                gender: signUpDto.Gender,
                passwordSalt: hashResult.Item1,
                passwordHash: hashResult.Item2,
                emailVerificationToken: _utilService.GenerateRandomString(25),
                emailVerificationTokenExpires: DateTime.Now.AddHours(_configuration.GetValue<int>("EmailVerificationTokenExpiresInMinutes"))
                );

            _userDal.Add(user);

            //Sending email verification link
            _emailService.SendEmailVerificationLink(user);

            //JWT will be returned
            return new SuccessDataResult<string>(Message.UserCreated, _jwtHelper.GenerateJwt(user));

        }
        public Result SignIn(SignInDto signInDto)
        {
            SignInDtoValidator signInValidator = new();
            ValidationResult result = signInValidator.Validate(signInDto);

            if(result.IsValid == false)
            {
                return new ErrorDataResult<List<ValidationFailure>>(result.Errors);
            }

            User user = _context.Users.FirstOrDefault(u => u.Email == signInDto.Email);

            if(user == null)
            {
                return new ErrorResult(Message.UserNotFound);
            }

            bool verifyPassword = _hashHelper.VerifyPassword(user.PasswordSalt, user.PasswordHash, signInDto.Password);

            if (!verifyPassword)
            {
                return new ErrorResult(Message.InvalidCredentials);
            }

            //if(user.IsVisible == false)
            //{
            //    user.IsVisible = true;

            //    _context.Update(user);
            //    _context.SaveChanges();
            //}

            return new SuccessDataResult<string>(_jwtHelper.GenerateJwt(user));
        }

        public Result SendEmailVerificationLink(SendEmailVerificationLinkDto sendEmailVerificationLinkDto)
        {

            SendEmailVerificationLinkDtoValidator validator = new();
            ValidationResult result = validator.Validate(sendEmailVerificationLinkDto);

            if (!result.IsValid)
            {
                return new ErrorDataResult<List<ValidationFailure>>(result.Errors);
            }


            User user = _context.Users.FirstOrDefault(u => u.Email == sendEmailVerificationLinkDto.Email);

            if(user == null)
            {
                return new ErrorResult(Message.UserNotFound);
            }

            _emailService.SendEmailVerificationLink(user);

            return new SuccessResult(Message.EmailVerificationLinkSent);
        }

        public Result VerifyEmail(string emailVerificationToken)
        {

            if(emailVerificationToken == null)
            {
                return new ErrorResult(Message.EmailVerificationTokenNull);
            }

            User user = _context.Users.SingleOrDefault(u => u.EmailVerificationToken == emailVerificationToken);

            if(user == null)
            {
                return new ErrorResult(Message.UserNotFound);
            }

            if (DateTime.UtcNow > user.EmailVerificationTokenExpires)
            {
                return new ErrorResult(Message.EmailVerificatioTokenExpired);
            }

            user.EmailVerificationToken = "";
            user.EmailVerificationTokenExpires = DateTime.UtcNow;

            _userDal.Update(user);

            return new SuccessResult(Message.EmailVerified);
        }

        public Result ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            ForgotPasswordDtoValidator validator = new();
            ValidationResult result = validator.Validate(forgotPasswordDto);

            if(result.IsValid != true)
            {
                return new ErrorDataResult<List<ValidationFailure>>(result.Errors);
            }

            User user = _context.Users.FirstOrDefault(u => u.Email == forgotPasswordDto.Email);

            if(user == null)
            {
                return new ErrorResult(Message.UserNotFound);
            }

            _emailService.SendResetPasswordLink(user);

            return new SuccessResult(Message.ResetPasswordLinkSent);
        }
    }
}
