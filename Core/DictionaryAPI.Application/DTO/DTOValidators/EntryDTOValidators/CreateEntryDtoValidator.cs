using DictionaryAPI.Application.DTO.DTOs.EntryDTOs;
using DictionaryAPI.Application.Utils.Constants;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.DTO.DTOValidators.EntryDTOValidators
{
    public class CreateEntryDtoValidator : AbstractValidator<CreateEntryDto>
    {
        public CreateEntryDtoValidator()
        {
            RuleFor(e => e.Content).NotNull().WithMessage(Message.ContentNotNull);
        }
    }
}
