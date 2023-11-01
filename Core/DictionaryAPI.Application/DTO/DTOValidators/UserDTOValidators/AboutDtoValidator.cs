using DictionaryAPI.Application.DTO.DTOs.UserDTOs;
using DictionaryAPI.Application.Utils.Constants;
using FluentValidation;
using FluentValidation.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.DTO.DTOValidators.UserDTOValidators
{
    public class AboutDtoValidator : AbstractValidator<AboutDto>
    {
        public AboutDtoValidator()
        {
            RuleFor(a => a.About).NotNull().WithMessage(Message.AboutNotNull);
        }
    }
}
