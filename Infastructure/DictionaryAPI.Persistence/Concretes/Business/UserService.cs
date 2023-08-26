using Azure.Core;
using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Application.Abstracts.Security.Hash;
using DictionaryAPI.Application.Abstracts.Security.JWT;
using DictionaryAPI.Application.Abstracts.Services.EmailService;
using DictionaryAPI.Application.DTO.DTOs.UserDTOs;
using DictionaryAPI.Application.DTO.DTOValidators.UserDTOValidators;
using DictionaryAPI.Application.Utils;
using DictionaryAPI.Application.Utils.Constants;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Entities;
using DictionaryAPI.Persistence.Contexts;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

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
        IHttpContextAccessor _contextAccesor;
        public UserService(IHashHelper hashHelper, IUserDal userDal, IConfiguration configuration, IUtilService utilService, IEmailService emailService, IJwtHelper jwtHelper, DictionaryContext context, IHttpContextAccessor contextAccesor)
        {
            _hashHelper = hashHelper;
            _userDal = userDal;
            _configuration = configuration;
            _utilService = utilService;
            _emailService = emailService;
            _jwtHelper = jwtHelper;
            _context = context;
            _contextAccesor = contextAccesor;
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

            if(user.IsVisible == false)
            {
                user.IsVisible = true;
                _userDal.Update(user);
            }

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
            user.IsEmailVerified = true;

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

        public Result ResetPassword(ResetPasswordDto resetPasswordDto, string resetPasswordToken)
        {
            ResetPasswordDtoValidator validator = new();
            ValidationResult result = validator.Validate(resetPasswordDto);

            if(result.IsValid != true)
            {
                return new ErrorDataResult<List<ValidationFailure>>(result.Errors);
            }

            if(resetPasswordToken == null)
            {
                return new ErrorResult(Message.ResetPasswordTokenNull);
            }

            User user = _context.Users.SingleOrDefault(u => u.ResetPasswordToken == resetPasswordToken);

            if(user == null)
            {
                return new ErrorResult(Message.UserNotFound);
            }

            if(resetPasswordDto.Password != resetPasswordDto.PasswordRepeat)
            {
                return new ErrorResult(Message.PasswordsDoNotMatch);
            }


            Tuple<byte[], byte[]> hash = _hashHelper.GenerateHash(resetPasswordDto.Password);
            
            user.ResetPasswordToken = "";
            user.ResetPasswordTokenExpires = DateTime.UtcNow;

            user.PasswordSalt = hash.Item1;
            user.PasswordHash = hash.Item2;

            _userDal.Update(user);

            return new SuccessResult(Message.PasswordChanged);
        }

        public Result PasswordChange(PasswordChangeDto passwordChangeDto)
        {
            PasswordChangeDtoValidator validator = new();
            ValidationResult result = validator.Validate(passwordChangeDto);

            if(result.IsValid != true)
            {
                return new ErrorDataResult<List<ValidationFailure>>(result.Errors);
            }

            if (passwordChangeDto.NewPassword != passwordChangeDto.NewPasswordRepeat)
            {
                return new ErrorResult(Message.PasswordsDoNotMatch);
            }

            User user = _userDal.GetSingle(u => u.Id == Guid.Parse(Convert.ToString(_contextAccesor.HttpContext.Items["Id"])));

            bool verify = _hashHelper.VerifyPassword(user.PasswordSalt, user.PasswordHash, passwordChangeDto.OldPassword);

            if (verify != true)
            {
                return new ErrorResult(Message.InvalidCredentials);
            }

            Tuple<byte[], byte[]> hash = _hashHelper.GenerateHash(passwordChangeDto.NewPassword);

            user.PasswordSalt = hash.Item1;
            user.PasswordHash = hash.Item2;

            _userDal.Update(user);

            return new SuccessResult(Message.PasswordChanged);
        }

        public Result DeactiveAccount(DeactivateAccountDto deactiveAccountDto)
        {

            DeactivateAccountDtoValidator validator = new();
            ValidationResult result = validator.Validate(deactiveAccountDto);

            if (result.IsValid != true)
            {
                return new ErrorDataResult<List<ValidationFailure>>(result.Errors);
            }

            User user = _userDal.GetSingle(u => u.Id == Guid.Parse(Convert.ToString(_contextAccesor.HttpContext.Items["Id"])));

            bool verify = _hashHelper.VerifyPassword(user.PasswordSalt, user.PasswordHash, deactiveAccountDto.Password);

            if(verify != true)
            {
                return new ErrorResult(Message.InvalidCredentials);
            }

            user.IsVisible = false;
            _userDal.Update(user);

            return new SuccessResult(Message.AccountDeactivated);
        }
    }
}
