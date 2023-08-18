using DictionaryAPI.Application.Utils;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Infastructure.Utils
{
    public class UtilService : IUtilService
    {

        IConfiguration _configuration;

        public UtilService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateRandomString(int size)
        {
            Random random = new();
            StringBuilder sb = new();

            string text = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            for (int i = 0; i < size; i++)
            {
                int randomNumber = random.Next(text.Length);
                sb.Append(text[randomNumber]);
            }

            return sb.ToString();

        }
        public string GenerateEmailVerificationLink(string emailVerificationToken)
        {
            string domain = _configuration.GetValue<string>("DevDomain");

            return $"{domain}/api/email/verification/verify?emailVerificationToken={emailVerificationToken}";
        }

        public string GenerateResetPasswordLink(string resetPasswordToken)
        {
            string domain = _configuration.GetValue<string>("DevDomain");

            return $"{domain}/api/password/reset?resetPasswordToken={resetPasswordToken}";
        }
    }
}
