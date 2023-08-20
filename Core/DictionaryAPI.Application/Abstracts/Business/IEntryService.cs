using DictionaryAPI.Application.DTO.DTOs.EntryDTOs;
using DictionaryAPI.Application.Utils.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.Abstracts.Business
{
    public interface IEntryService
    {
        Result CreateEntry(CreateEntryDto createEntryDto, string titleSlug);
        Result DeleteEntry(string entryId);
        Result HideEntry(string entryId);
    }
}
