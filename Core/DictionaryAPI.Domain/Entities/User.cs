using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Domain.Entities
{
    public enum Gender
    {

    }
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string? About { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public bool IsAdmin { get; set; } = false;
        public string ProfileImageUrl { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpires { get; set; }
        public bool IsEmailVerified { get; set; } = false;

        public ICollection<Entry> Entries { get; set; }

        public User(string username, string email, DateTime birthDate, Gender gender)
        {
            Username = username;
            Email = email;
            BirthDate = birthDate;
            Gender = gender;
        }

    }
}
