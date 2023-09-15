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
    public class TwoFactorAuthDtoValidator : AbstractValidator<TwoFactorAuthDto>
    {
        public TwoFactorAuthDtoValidator()
        {
            RuleFor(e => e.AuthCode).NotNull().WithMessage(Message.AuthCodeNotNull);
        }
    }
}