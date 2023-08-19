using DictionaryAPI.Application.DTO.DTOs.UserDTOs;
using DictionaryAPI.Application.Utils.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.DTO.DTOValidators.UserDTOValidators
{
    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator()
        {
            RuleFor(r => r.Password).NotNull().WithMessage(Message.PasswordNotNull);
            RuleFor(r => r.Password).Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$").WithMessage(Message.InvalidPasswordFormat);


            RuleFor(r => r.PasswordRepeat).NotNull().WithMessage(Message.PasswordNotNull);
            RuleFor(r => r.PasswordRepeat).Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$").WithMessage(Message.InvalidPasswordFormat);
        }
    }
}