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
    public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
    {
        public ForgotPasswordDtoValidator()
        {
            RuleFor(f => f.Email).NotNull().WithMessage(Message.EmailNotNull);
            RuleFor(f => f.Email).EmailAddress().WithMessage(Message.InvalidCredentials);
        }
    }
}
