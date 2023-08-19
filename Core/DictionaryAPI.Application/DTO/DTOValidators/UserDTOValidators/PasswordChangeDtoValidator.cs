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
    public class PasswordChangeDtoValidator : AbstractValidator<PasswordChangeDto>
    {
        public PasswordChangeDtoValidator()
        {
            RuleFor(p => p.OldPassword).NotNull().WithMessage(Message.PasswordNotNull);

            RuleFor(p => p.NewPassword).NotNull().WithMessage(Message.PasswordNotNull);
            RuleFor(r => r.NewPassword).Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$").WithMessage(Message.InvalidPasswordFormat);

            RuleFor(p => p.NewPasswordRepeat).NotNull().WithMessage(Message.PasswordNotNull);
            RuleFor(r => r.NewPasswordRepeat).Matches(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$").WithMessage(Message.InvalidPasswordFormat);

        }
    }
}
