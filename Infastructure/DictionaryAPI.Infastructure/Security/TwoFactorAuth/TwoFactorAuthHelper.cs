using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Application.Abstracts.Security.TwoFactorAuth;
using DictionaryAPI.Application.DTO.DTOs.UserDTOs;
using DictionaryAPI.Application.DTO.DTOValidators.UserDTOValidators;
using DictionaryAPI.Domain.Entities;
using FluentValidation.Results;
using Google.Authenticator;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Infastructure.Security.TwoFactorAuth
{
    public class TwoFactorAuthHelper : ITwoFactorAuthHelper
    {
        IHttpContextAccessor _contextAccessor;
        IUserDal _userDal;

        //To be refactored
        public TwoFactorAuthHelper(IHttpContextAccessor contextAccessor, IUserDal userDal)
        {
            _contextAccessor = contextAccessor;
            _userDal = userDal;
        }

        private bool TwoFactorAuthDtoValidation(TwoFactorAuthDto twoFactorAuthDto)
        {
            TwoFactorAuthDtoValidator validator = new();
            ValidationResult result = validator.Validate(twoFactorAuthDto);

            if (result.IsValid != true)
            {
                return false;
            }

            return true;
        }

        private bool VerifyAuthCode(byte[] twoFactorSecretKey, string authCode)
        {
            TwoFactorAuthenticator twoFactorAuth = new();
            return twoFactorAuth.ValidateTwoFactorPIN(twoFactorSecretKey, authCode);
        }

        public bool ValidateAuthCode(byte[] twoFactorSecretKey, TwoFactorAuthDto twoFactorAuthDto)
        {

            if(TwoFactorAuthDtoValidation(twoFactorAuthDto) == false || twoFactorSecretKey == null)
            {
                return false;
            }

            return VerifyAuthCode(twoFactorSecretKey, twoFactorAuthDto.AuthCode);

        }

        public bool ValidateAuthCode(TwoFactorAuthDto twoFactorAuthDto)
        {

            if (TwoFactorAuthDtoValidation(twoFactorAuthDto) == false)
            {
                return false;
            }

            User user = _userDal.GetSingle(
                u => u.Id == Guid.Parse(Convert.ToString(_contextAccessor.HttpContext.Items["Id"]))
            );

            return VerifyAuthCode(user.TwoFactorSecretKey, twoFactorAuthDto.AuthCode);
        }
    }
}
