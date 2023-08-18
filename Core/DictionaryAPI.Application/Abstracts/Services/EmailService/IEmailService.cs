using DictionaryAPI.Application.DTO.DTOs;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.Abstracts.Services.EmailService
{
    public interface IEmailService
    {
        void SendMail(List<string> recipients, string? subject, string body);
        void SendEmailVerificationLink(User user);
        void SendResetPasswordLink(User user);
    }
}
