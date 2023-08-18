using DictionaryAPI.Application.DTO.DTOs;
using DictionaryAPI.Application.Utils.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.DTO.DTOValidators
{
    public class SendEmailVerificationLinkDtoValidator : AbstractValidator<SendEmailVerificationLinkDto>
    {
        public SendEmailVerificationLinkDtoValidator()
        {
            RuleFor(s => s.Email).NotNull().WithMessage(Message.EmailNotNull);
            RuleFor(s => s.Email).EmailAddress().WithMessage(Message.InvalidEmail);
        }
    }
}
