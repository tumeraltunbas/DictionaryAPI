using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Application.Abstracts.Services.EmailService;
using DictionaryAPI.Application.DTO.DTOs;
using DictionaryAPI.Application.DTO.DTOValidators;
using DictionaryAPI.Application.Utils;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Entities;
using DictionaryAPI.Persistence.Contexts;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace DictionaryAPI.Infastructure.Services.EmailService
{
    public class EmailService : IEmailService
    {
        readonly IConfiguration _configuration;
        readonly IUtilService _utilService;
        //readonly DictionaryContext _context;
        IUserDal _userDal;
        public EmailService(IConfiguration configuration, IUtilService utilService, IUserDal userDal)
        {
            _configuration = configuration;
            _utilService = utilService;
            //_context = context;
            _userDal = userDal;
        }

        public void SendMail(List<string> recipients, string? subject, string body)
        {
            
            //Getting values from appsettings.json
            IConfigurationSection smtpConfig = _configuration.GetSection("SmtpConfig");

            //SMTP configuration
            SmtpClient smtp = new(host: smtpConfig["SmtpHost"], port: Int32.Parse(smtpConfig["SmtpPort"]));
            smtp.Credentials = new NetworkCredential(userName: smtpConfig["SmtpUser"], password: smtpConfig["SmtpPassword"]);
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;

            //MailMessage configuration
            MailMessage mailMessage = new();
            mailMessage.From = new MailAddress(smtpConfig["SmtpUser"]);
            foreach (var item in recipients)
            {
                mailMessage.To.Add(new MailAddress(item));
            
            }
            mailMessage.Subject = subject;
            mailMessage.Body = body;
           
            smtp.Send(mailMessage);

        }

        public void SendEmailVerificationLink(User user)
        {
            string token = _utilService.GenerateRandomString(25);
            string link = _utilService.GenerateEmailVerificationLink(token);

            int emailVerificationTokenExpiresInMinutes = _configuration.GetValue<int>("EmailVerificationTokenExpiresInMinutes");

            user.EmailVerificationToken = token;
            user.EmailVerificationTokenExpires = DateTime.UtcNow.AddMinutes(emailVerificationTokenExpiresInMinutes);

            //_context.Users.Update(user);
            _userDal.Update(user);

            SendMail(
                recipients: new List<string>() { user.Email },
                subject: "Email Verification Link",
                body: $"Dear {user.Username}, your email verification link is {link}. This link is valid for {emailVerificationTokenExpiresInMinutes} minutes"
            );
                
        }
    }
}
