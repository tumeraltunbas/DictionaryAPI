using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.Utils.Constants
{
    public static class Message
    {
        public static string InvalidEmail = "Please provide a valid email";
        public static string EmailNotNull = "Email can not be null";
        public static string UsernameNotNull = "Username can not be null";
        public static string PasswordNotNull = "Password can not be null";
        public static string InvalidPasswordFormat = "Password must contain: Minimum eight characters, at least one letter and one number";
        public static string BirthDateNotNull = "Birth Date can not be null";
        public static string GenderNotNull = "Gender can not null";
     }
}
