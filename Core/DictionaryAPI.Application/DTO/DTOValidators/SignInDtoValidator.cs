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
    public class SignInDtoValidator : AbstractValidator<SignInDto>
    {
        public SignInDtoValidator()
        {
            RuleFor(s => s.Email).NotNull().WithMessage(Message.EmailNotNull);
            RuleFor(s => s.Email).EmailAddress().WithMessage(Message.InvalidEmail);

            RuleFor(s => s.Password).NotNull().WithMessage(Message.PasswordNotNull);
        }
    }
}
