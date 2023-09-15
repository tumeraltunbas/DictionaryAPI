using DictionaryAPI.Application.DTO.DTOs.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.Abstracts.Security.TwoFactorAuth
{
    public interface ITwoFactorAuthHelper
    {
        bool ValidateAuthCode(byte[] twoFactorSecret, TwoFactorAuthDto twoFactorAuthDto);
        bool ValidateAuthCode(TwoFactorAuthDto twoFactorAuthDto);
    }
}
