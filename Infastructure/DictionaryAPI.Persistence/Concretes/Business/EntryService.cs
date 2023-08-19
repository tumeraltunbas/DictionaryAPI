using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Application.DTO.DTOs.EntryDTOs;
using DictionaryAPI.Application.DTO.DTOValidators.EntryDTOValidators;
using DictionaryAPI.Application.Utils.Constants;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Entities;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Persistence.Concretes.Business
{
    public class EntryService : IEntryService
    {
        ITitleDal _titleDal;
        IEntryDal _entryDal;

        public EntryService(ITitleDal titleDal, IEntryDal entryDal)
        {
            _titleDal = titleDal;
            _entryDal = entryDal;
        }

        public Result CreateEntry(CreateEntryDto createEntryDto, IDictionary<object, object> items, string titleSlug)
        {

            CreateEntryDtoValidator validator = new();
            ValidationResult result = validator.Validate(createEntryDto);

            if(result.IsValid != true)
            {
                return new ErrorDataResult<List<ValidationFailure>>(result.Errors);
            }

            Title title;
            title = _titleDal.GetBySlug(titleSlug);

            if(title == null)
            {
                title = new(content: titleSlug.Replace("-", " "));
                _titleDal.Add(title);
            }

            Entry entry = new(
                content: createEntryDto.Content,
                titleId: title.Id,
                userId: Guid.Parse(Convert.ToString(items["Id"]))
            );

            _entryDal.Add(entry);

            return new SuccessResult(Message.EntryCreated);    
        }
    }
}
