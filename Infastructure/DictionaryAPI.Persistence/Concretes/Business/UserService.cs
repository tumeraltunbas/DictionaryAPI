using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Application.Abstracts.Security.Hash;
using DictionaryAPI.Application.Abstracts.Services.EmailService;
using DictionaryAPI.Application.DTO.DTOs;
using DictionaryAPI.Application.DTO.DTOValidators;
using DictionaryAPI.Application.Utils;
using DictionaryAPI.Application.Utils.Constants;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Entities;
using FluentValidation.Results;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Net.Mail;
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
        public UserService(IHashHelper hashHelper, IUserDal userDal, IConfiguration configuration, IUtilService utilService, IEmailService emailService)
        {
            _hashHelper = hashHelper;
            _userDal = userDal;
            _configuration = configuration;
            _utilService = utilService;
            _emailService = emailService;
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
            return new SuccessResult(Message.UserCreated);

        }
    }
}
