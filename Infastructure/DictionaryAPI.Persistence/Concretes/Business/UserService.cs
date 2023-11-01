using Azure.Core;
using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Application.Abstracts.Security.Hash;
using DictionaryAPI.Application.Abstracts.Security.JWT;
using DictionaryAPI.Application.Abstracts.Security.TwoFactorAuth;
using DictionaryAPI.Application.Abstracts.Services.EmailService;
using DictionaryAPI.Application.Abstracts.Services.StorageService;
using DictionaryAPI.Application.DTO.DTOs.UserDTOs;
using DictionaryAPI.Application.DTO.DTOValidators.UserDTOValidators;
using DictionaryAPI.Application.Utils;
using DictionaryAPI.Application.Utils.Constants;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Entities;
using DictionaryAPI.Persistence.Contexts;
using FluentValidation.Results;
using Google.Authenticator;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Text;

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
        ITwoFactorAuthHelper _twoFactorAuthHelper;
        IStorageService _storageService;
        IWebHostEnvironment _hostEnvironment;
        public UserService(
            IHashHelper hashHelper,
            IUserDal userDal,
            IConfiguration configuration,
            IUtilService utilService,
            IEmailService emailService,
            IJwtHelper jwtHelper,
            DictionaryContext context,
            IHttpContextAccessor contextAccesor,
            ITwoFactorAuthHelper twoFactorAuthHelper,
            IStorageService storageService,
            IWebHostEnvironment hostEnvironment)
        {
            _hashHelper = hashHelper;
            _userDal = userDal;
            _configuration = configuration;
            _utilService = utilService;
            _emailService = emailService;
            _jwtHelper = jwtHelper;
            _context = context;
            _contextAccesor = contextAccesor;
            _twoFactorAuthHelper = twoFactorAuthHelper;
            _storageService = storageService;
            _hostEnvironment = hostEnvironment;
        }

        public Result SignUp(SignUpDto signUpDto)
        {

            SignUpDtoValidator validator = new();
            ValidationResult result = validator.Validate(signUpDto);

            if (result.IsValid == false)
            {
                return new ErrorDataResult<List<ValidationFailure>>(result.Errors);
            }

            Tuple<byte[], byte[]> hashResult = _hashHelper.GenerateHash(signUpDto.Password);

            User user = new(
                username: signUpDto.Username,
                email: signUpDto.Email,
                birthDate: signUpDto.BirthDate,
                gender: signUpDto.Gender,
                passwordSalt: hashResult.Item1,
                passwordHash: hashResult.Item2,
                emailVerificationToken: _utilService.GenerateRandomString(25),
                emailVerificationTokenExpires: DateTime.UtcNow.AddHours(_configuration.GetValue<int>("EmailVerificationTokenExpiresInMinutes"))
                );

            _userDal.Add(user);

            _emailService.SendEmailVerificationLink(user);

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

            if (user.IsTwoFactorAuthEnabled == true)
            {
                return new SuccessResult();
            }
            else
            {
                return new SuccessDataResult<string>(_jwtHelper.GenerateJwt(user));
            }

        }

        public Result SendEmailVerificationLink(EmailDto sendEmailVerificationLinkDto)
        {

            EmailDtoValidator validator = new();
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

        public Result GetTwoFactorAuthCredentials()
        {
            User user = _userDal.GetSingle(
                u => u.Id == Guid.Parse(Convert.ToString(_contextAccesor.HttpContext.Items["Id"]))
            );

            if(user.IsTwoFactorAuthEnabled == true)
            {
                return new ErrorResult(Message.TwoFactorAuthAlreadyEnabled);
            }

            if(user.TwoFactorSecretKey == null)
            {
                user.TwoFactorSecretKey = Encoding.UTF8.GetBytes(_utilService.GenerateRandomString(40));
                _userDal.Update(user);
            }

            TwoFactorAuthenticator twoFactorAuth = new();
            SetupCode setupCode = twoFactorAuth.GenerateSetupCode(_configuration.GetValue<string>("ProjectName"), user.Email, user.TwoFactorSecretKey);

            var responseData = new
            {
                manualEntryKey = setupCode.ManualEntryKey,
                qrCode = setupCode.QrCodeSetupImageUrl
            };

            return new SuccessDataResult<object>(responseData);
        }

        public Result EnableTwoFactorAuth(TwoFactorAuthDto enableTwoFactorAuthDto)
        {

            User user = _userDal.GetSingle(
                u => u.Id == Guid.Parse(Convert.ToString(_contextAccesor.HttpContext.Items["Id"]))
            );

            if (user.IsTwoFactorAuthEnabled == true)
            {
                return new ErrorResult(Message.TwoFactorAuthAlreadyEnabled);
            }

            bool validation = _twoFactorAuthHelper.ValidateAuthCode(user.TwoFactorSecretKey, enableTwoFactorAuthDto);

            if(validation == false)
            {
                return new ErrorResult(Message.InvalidAuthCode);
            }

            user.IsTwoFactorAuthEnabled = true;

            _userDal.Update(user);

            return new SuccessResult(Message.TwoFactorAuthEnabled);

        }

        public Result ValidateTwoFactorAuth(TwoFactorAuthDto validateTwoFactorAuthDto)
        {

            User user;

            if (_contextAccesor.HttpContext.Items["Id"] != null && validateTwoFactorAuthDto.Email == null)
            {
                //User from request
                user = _userDal.GetSingle(
                        u => u.Id == Guid.Parse(Convert.ToString(_contextAccesor.HttpContext.Items["Id"]))
                    );
            }
            else
            {
                //User from email
                user = _userDal.GetSingle(
                        u => u.Email == validateTwoFactorAuthDto.Email
                    );
            }

            if(user == null)
            {
                return new ErrorResult(Message.UserNotFound);
            }

            bool validation = _twoFactorAuthHelper.ValidateAuthCode(user.TwoFactorSecretKey, validateTwoFactorAuthDto);

            if (validation == false)
            {
                return new ErrorResult(Message.InvalidAuthCode);
            }

            if (validateTwoFactorAuthDto.Email != null) 
            {
                return new SuccessDataResult<string>(_jwtHelper.GenerateJwt(user));
            }

            return new SuccessResult();

        }

        public Result DisableTwoFactorAuth(TwoFactorAuthDto disableTwoFactorAuthDto)
        {

            User user = _userDal.GetSingle(
                u => u.Id == Guid.Parse(Convert.ToString(_contextAccesor.HttpContext.Items["Id"]))
            );      

            if(user.IsTwoFactorAuthEnabled == false)
            {
                return new ErrorResult(Message.TwoFactorAuthAlreadyDisabled);
            }

            bool validation = _twoFactorAuthHelper.ValidateAuthCode(user.TwoFactorSecretKey, disableTwoFactorAuthDto);

            if (validation == false)
            {
                return new ErrorResult(Message.InvalidAuthCode);
            }

            user.IsTwoFactorAuthEnabled = false;
            user.TwoFactorSecretKey = null;

            _userDal.Update(user);

            return new SuccessResult(Message.TwoFactorAuthDisabled);

        }

        public Result GetProfile(string username)
        {

            var user = _context.Users
                        .Include(u => u.Entries)
                        .Include(u => u.VotedEntries)
                        .Include(u => u.FavoritedEntries)
                        .Select(u => new
                        {
                            u.Id,
                            u.Username,
                            u.About,
                            u.Gender,
                            u.BirthDate,
                            u.ProfileImageUrl,
                            u.CreatedAt,
                            u.Entries,
                            u.VotedEntries,
                            u.FavoritedEntries
                        })
                        .FirstOrDefault(u => u.Username == username);

            if(user == null)
            {
                return new ErrorResult(Message.UserNotFound);
            }

            return new SuccessDataResult<object>(user);
                
        }

        public Result EmailChange(EmailDto emailChangeDto)
        {

            EmailDtoValidator validator = new();
            ValidationResult result = validator.Validate(emailChangeDto);

            if(result.IsValid != true)
            {
                return new ErrorDataResult<List<ValidationFailure>>(result.Errors);
            }


            User user = _userDal.GetSingle(
                u => u.Id == Guid.Parse(Convert.ToString(_contextAccesor.HttpContext.Items["Id"]))
            );

            //Unique check for email, the application stops.

            if (user.Email == emailChangeDto.Email)
            {
                return new ErrorResult(Message.EmailSame);
            }

            user.Email = emailChangeDto.Email;

            if (user.IsEmailVerified)
            {
                user.IsEmailVerified = false;
            }
            
            _userDal.Update(user);

            EmailDto emailDto = new EmailDto { Email = user.Email };
            SendEmailVerificationLink(emailDto);

            return new SuccessResult(Message.EmailChanged);

        }

        public Result UploadProfileImage(IFormFile file)
        {
            if(file == null)
            {
                return new ErrorResult(Message.FileNull);
            }

            User user = _userDal.GetSingle(
                u => u.Id == Guid.Parse(Convert.ToString(_contextAccesor.HttpContext.Items["Id"]))
             );

            string uploadPath = Path.Combine(_hostEnvironment.WebRootPath, "images/profile-images");

            string fileName = _storageService.UploadFile(file, uploadPath);

            user.ProfileImageUrl = fileName;
            _userDal.Update(user);

            return new SuccessResult(Message.ProfileImageUploaded);
        }
    }
}