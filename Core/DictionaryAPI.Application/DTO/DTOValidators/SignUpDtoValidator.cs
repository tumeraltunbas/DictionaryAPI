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
    public class SignUpDtoValidator : AbstractValidator<SignUpDto>
    {
        public SignUpDtoValidator()
        {
            RuleFor(s => s.Email).EmailAddress().WithMessage(Message.InvalidEmail);
            RuleFor(s => s.Email).NotNull().WithMessage(Message.EmailNotNull);

            RuleFor(s => s.Username).NotNull().WithMessage(Message.UsernameNotNull);

            RuleFor(s => s.Password).NotNull().WithMessage(Message.PasswordNotNull);
            RuleFor(s => s.Password).Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$").WithMessage(Message.InvalidPasswordFormat);

            RuleFor(s => s.BirthDate).NotNull().WithMessage(Message.BirthDateNotNull);
            RuleFor(s => s.Gender).NotNull().WithMessage(Message.GenderNotNull)
        }
    }
}
