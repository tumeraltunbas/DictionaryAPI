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
        public static string GenderNotNull = "Gender can not be null";

        //--------
        public static string EmailFromNotNull = "From can not be null";
        public static string EmailRecipientsNotNull = "Recipients can not be null";

        public static string UserCreated = "User has been successfully created";
        public static string UserNotFound = "User not found";
        public static string InvalidCredentials = "Invalid credentials";
        public static string EmailVerificationLinkSent = "Email verification link has been successfully sent";
        public static string EmailVerificationTokenNull = "Email verification token can not be null";
        public static string EmailVerificatioTokenExpired = "Email verification token expired";
        public static string EmailVerified = "Email has been successfully verified";
        public static string ResetPasswordLinkSent = "Reset password link has been successfully sent";
        public static string PasswordsDoNotMatch = "Passwords do not match";
        public static string PasswordChanged = "Password has been successfully changed";
        public static string ResetPasswordTokenNull = "Reset password token can not be null";
        public static string AccountDeactivated = "Account has been successfully deactivated";
        public static string ContentNotNull = "Content can not be null";
        public static string EntryCreated = "Entry has been successfully created";
    }
}
