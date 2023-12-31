﻿using DictionaryAPI.Application.DTO.DTOs.UserDTOs;
using DictionaryAPI.Application.Utils.Result;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.Abstracts.Business
{
    public interface IUserService
    {
        Result SignUp(SignUpDto signUpDto);
        Result SignIn(SignInDto signInDto);
        Result SendEmailVerificationLink(EmailDto sendEmailVerificationLinkDto);
        Result VerifyEmail(string url);
        Result ForgotPassword(ForgotPasswordDto forgotPasswordDto);
        Result ResetPassword(ResetPasswordDto resetPasswordDto, string resetPasswordToken);
        Result PasswordChange(PasswordChangeDto passwordChangeDto);
        Result DeactiveAccount(DeactivateAccountDto deactiveAccountDto);
        Result GetTwoFactorAuthCredentials();
        Result EnableTwoFactorAuth(TwoFactorAuthDto enableTwoFactorAuthDto);
        Result ValidateTwoFactorAuth(TwoFactorAuthDto validateTwoFactorAuthDto);
        Result DisableTwoFactorAuth(TwoFactorAuthDto disableTwoFactorAuthDto);
        Result GetProfile(string username);
        Result EmailChange(EmailDto emailChangeDto);
        Result UploadProfileImage(IFormFile file);
        Result UpdateAbout(AboutDto aboutDto);
    }
}
