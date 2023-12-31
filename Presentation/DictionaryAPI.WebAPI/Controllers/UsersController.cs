﻿using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Infastructure.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DictionaryAPI.WebAPI.CustomAttributes;
using DictionaryAPI.Application.DTO.DTOs.UserDTOs;

namespace DictionaryAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        IUserService _userService;
        IWebHostEnvironment _webHostEnvironment;
        public UsersController(IUserService userService, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpPost("sign/up")]
        public IActionResult SignUp([FromBody] SignUpDto signUpDto)
        {
            var result = _userService.SignUp(signUpDto);

            if (result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("sign/in")]
        public IActionResult SignIn([FromBody] SignInDto signInDto)
        {
            var result = _userService.SignIn(signInDto);

            if (result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("email/verification/send")]
        public IActionResult SendEmailVerificationLink([FromBody] EmailDto sendEmailVerificationLinkDto)
        {
            var result = _userService.SendEmailVerificationLink(sendEmailVerificationLinkDto);
            
            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("email/verification/verify")]
        public IActionResult VerifyEmail([FromQuery] string emailVerificationToken)
        {
            var result = _userService.VerifyEmail(emailVerificationToken);
            
            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("password/forgot")]
        public IActionResult ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var result = _userService.ForgotPassword(forgotPasswordDto);
            
            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("password/reset")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDto resetPasswordDto, [FromQuery] string resetPasswordToken)
        {
            var result = _userService.ResetPassword(resetPasswordDto, resetPasswordToken);

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [RegisterJwtClaimsToItems]
        [HttpPut("password/change")]
        public IActionResult PasswordChange(PasswordChangeDto passwordChangeDto)
        {
            var result = _userService.PasswordChange(passwordChangeDto);

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [RegisterJwtClaimsToItems]
        [HttpPut("account/deactivate")]
        public IActionResult DeactivateAccount(DeactivateAccountDto deactivateAccountDto)
        {
            var result = _userService.DeactiveAccount(deactivateAccountDto);

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [RegisterJwtClaimsToItems]
        [HttpGet("2fa/credentials")]
        public IActionResult GetTwoFactorAuthCredentials()
        {
            var result = _userService.GetTwoFactorAuthCredentials();

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [RegisterJwtClaimsToItems]
        [HttpPost("2fa/enable")]
        public IActionResult EnableTwoFactorAuth(TwoFactorAuthDto enableTwoFactorAuthDto)
        {
            var result = _userService.EnableTwoFactorAuth(enableTwoFactorAuthDto);

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        //[Authorize]
        [RegisterJwtClaimsToItems]
        [HttpPost("2fa/validate")]
        public IActionResult ValidateTwoFactorAuth(TwoFactorAuthDto validateTwoFactorAuth)
        {
            var result = _userService.ValidateTwoFactorAuth(validateTwoFactorAuth);
            Console.WriteLine();
            if (result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [RegisterJwtClaimsToItems]
        [HttpPost("2fa/disable")]
        public IActionResult DisableTwoFactorAuth(TwoFactorAuthDto disableTwoFactorAuth)
        {
            var result = _userService.DisableTwoFactorAuth(disableTwoFactorAuth);

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);

        }

        [HttpGet("{username}")]
        public IActionResult GetProfile(string username)
        {
            var result = _userService.GetProfile(username);

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [RegisterJwtClaimsToItems]
        [HttpPut("email/change")]
        public IActionResult EmailChange(EmailDto emailChangeDto)
        {
            var result = _userService.EmailChange(emailChangeDto);
            
            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [RegisterJwtClaimsToItems]
        [HttpPost("upload/pp")]
        public IActionResult UploadProfileImage()
        {
            var result = _userService.UploadProfileImage(Request.Form.Files[0]);

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [RegisterJwtClaimsToItems]
        [HttpPut("about/update")]
        public IActionResult UpdateAbout(AboutDto aboutDto)
        {
            var result = _userService.UpdateAbout(aboutDto);
            
            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
