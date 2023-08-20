using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Application.DTO.DTOs.EntryDTOs;
using DictionaryAPI.Application.DTO.DTOValidators.EntryDTOValidators;
using DictionaryAPI.Application.Utils.Constants;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Entities;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
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
        IHttpContextAccessor _contextAccessor;

        public EntryService(ITitleDal titleDal, IEntryDal entryDal, IHttpContextAccessor contextAccessor)
        {
            _titleDal = titleDal;
            _entryDal = entryDal;
            _contextAccessor = contextAccessor;
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

        public Result DeleteEntry(string entryId)
        {
            Entry entry = _entryDal.GetById(Guid.Parse(entryId));

            if (entry == null)
            {
                return new ErrorResult(Message.EntryNotFound);
            }

            if (entry.UserId != Guid.Parse(Convert.ToString(_contextAccessor.HttpContext.Items["Id"])))
            {
                return new ErrorResult(Message.UnAuthorized);
            }

            _entryDal.Delete(entry);

            return new SuccessResult(Message.EntryDeleted);
        }
    }
}
