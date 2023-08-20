using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.DTO.DTOs.EntryDTOs;
using DictionaryAPI.Application.Utils.Result;
using DictionaryAPI.WebAPI.CustomAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntriesController : ControllerBase
    {

        IEntryService _entryService;
        public EntriesController(IEntryService entryService)
        {
            _entryService = entryService;
        }

        [Authorize]
        [RegisterJwtClaimsToItems]
        [HttpPost("{titleSlug}/create")]
        public IActionResult CreateEntry(CreateEntryDto createEntryDto, string titleSlug)
        {
            var result = _entryService.CreateEntry(createEntryDto, titleSlug);

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [RegisterJwtClaimsToItems]
        [HttpDelete("{entrySlug}/delete")]
        public IActionResult DeleteEntry(string entrySlug)
        {
            var result = _entryService.DeleteEntry(entrySlug);

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
