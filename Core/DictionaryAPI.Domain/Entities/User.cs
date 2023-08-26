using DictionaryAPI.Domain.Abstract.Entity;
using DictionaryAPI.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;


namespace DictionaryAPI.Domain.Entities
{

    public class User : BaseEntity, IEntity
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string? About { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public bool IsAdmin { get; set; } = false;
        public string ProfileImageUrl { get; set; } = "default.jpg";
        public string? ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpires { get; set; }
        public string EmailVerificationToken { get; set; }
        public DateTime EmailVerificationTokenExpires { get; set; }
        public bool IsEmailVerified { get; set; } = false;


        public ICollection<Entry> Entries { get; set; }

        public ICollection<EntryFavorite> FavoritedEntries { get; set; }
        public ICollection<EntryVote> VotedEntries { get; set; }


        //Constructor
        public User(
            string username, 
            string email, 
            DateTime birthDate, 
            Gender gender, 
            byte[] passwordSalt, 
            byte[] passwordHash, 
            string emailVerificationToken, 
            DateTime emailVerificationTokenExpires)
        {
            Username = username;
            Email = email;
            BirthDate = birthDate;
            Gender = gender;
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
            EmailVerificationToken = emailVerificationToken;
            EmailVerificationTokenExpires = emailVerificationTokenExpires;
        }

    }
}
