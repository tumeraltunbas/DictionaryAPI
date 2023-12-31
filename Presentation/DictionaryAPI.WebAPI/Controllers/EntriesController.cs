﻿using DictionaryAPI.Application.Abstracts.Business;
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
        [HttpDelete("{entryId}/delete")]
        public IActionResult DeleteEntry(string entryId)
        {
            var result = _entryService.DeleteEntry(entryId);

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [RegisterJwtClaimsToItems]
        [HttpPut("{entryId}/hide")]
        public IActionResult HideEntry(string entryId)
        {
            var result = _entryService.HideEntry(entryId);

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [RegisterJwtClaimsToItems]
        [HttpPut("{entryId}/update")]
        public IActionResult UpdateEntry(UpdateEntryDto updateEntryDto, string entryId)
        {
            var result = _entryService.UpdateEntry(updateEntryDto, entryId);

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [Authorize]
        [RegisterJwtClaimsToItems]
        [HttpGet("")]
        public IActionResult GetEntriesByUser()
        {
            var result = _entryService.GetEntriesByUser();

            if (result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("{titleSlug}")]
        public IActionResult GetEntriesByTitle(string titleSlug)
        {
            var result = _entryService.GetEntriesByTitle(titleSlug);

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}
