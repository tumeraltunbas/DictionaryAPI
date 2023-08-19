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
    public class DeactivateAccountDtoValidator : AbstractValidator<DeactivateAccountDto>
    {
        public DeactivateAccountDtoValidator()
        {
            RuleFor(d => d.Password).NotNull().WithMessage(Message.PasswordNotNull);
        }
    }
}
