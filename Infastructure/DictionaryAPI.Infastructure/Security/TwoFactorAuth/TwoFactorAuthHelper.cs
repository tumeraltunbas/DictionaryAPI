using DictionaryAPI.Application.Abstracts.Security.TwoFactorAuth;
using Google.Authenticator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Infastructure.Security.TwoFactorAuth
{
    public class TwoFactorAuthHelper : ITwoFactorAuthHelper
    {
        public bool ValidateAuthCode(byte[] twoFactorSecretKey, string authCode)
        {
            TwoFactorAuthenticator twoFactorAuth = new();

            return twoFactorAuth.ValidateTwoFactorPIN(twoFactorSecretKey, authCode);
        }
    }
}
