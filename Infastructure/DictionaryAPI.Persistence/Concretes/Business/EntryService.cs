using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.Abstracts.DAL;
using DictionaryAPI.Application.DTO.DTOs.EntryDTOs;
using DictionaryAPI.Application.DTO.DTOValidators.EntryDTOValidators;
using DictionaryAPI.Application.Utils.Constants;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.Domain.Entities;
using DictionaryAPI.Persistence.Contexts;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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

        public Result CreateEntry(CreateEntryDto createEntryDto, string titleSlug)
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
                userId: Guid.Parse(Convert.ToString(_contextAccessor.HttpContext.Items["Id"]))
            );

            _entryDal.Add(entry);

            return new SuccessResult(Message.EntryCreated);    
        }

        public Result DeleteEntry(string entryId)
        {
            Entry entry = _entryDal.GetSingle(e => e.Id == Guid.Parse(entryId));

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

        public Result HideEntry(string entryId)
        {
            Entry entry = _entryDal.GetSingle(e => e.Id == Guid.Parse(entryId));

            if (entry == null)
            {
                return new ErrorResult(Message.EntryNotFound);
            }

            if(entry.UserId != Guid.Parse(Convert.ToString(_contextAccessor.HttpContext.Items["Id"])))
            {
                return new ErrorResult(Message.UnAuthorized);
            }

            entry.IsVisible = false;
            _entryDal.Update(entry);

            return new SuccessResult(Message.EntryHid);
        }

        public Result UpdateEntry(UpdateEntryDto updateEntryDto, string entryId)
        {

            UpdateEntryDtoValidator validator = new();
            ValidationResult result = validator.Validate(updateEntryDto);

            if (result.IsValid != true)
            {
                return new ErrorDataResult<List<ValidationFailure>>(result.Errors);
            }

            Entry entry = _entryDal.GetSingle(e => e.Id == Guid.Parse(entryId));

            if (entry == null || entry.IsVisible != true)
            {
                return new ErrorResult(Message.EntryNotFound);
            }

            if (entry.UserId != Guid.Parse(Convert.ToString(_contextAccessor.HttpContext.Items["Id"])))
            {
                return new ErrorResult(Message.UnAuthorized);
            }

            entry.Content = updateEntryDto.Content;
            _entryDal.Update(entry);

            return new SuccessResult(Message.EntryUpdated);

        }

        public DataResult<List<Entry>> GetEntriesByUser()
        {
            List<Entry> entries = _entryDal
                .GetAll(e => e.UserId == Guid.Parse(Convert.ToString(_contextAccessor.HttpContext.Items["Id"])))
                .Where(e => e.IsVisible == true)
                .ToList();

            return new SuccessDataResult<List<Entry>>(entries);
        }

        public DataResult<List<Entry>> GetEntriesByTitle(string titleSlug)
        {
            Title title = _titleDal.GetSingle(t => t.Slug == titleSlug);
            List<Entry> entries = _entryDal.GetAll(e => e.TitleId == title.Id);

            return new SuccessDataResult<List<Entry>>(entries);
        }
    }
}
