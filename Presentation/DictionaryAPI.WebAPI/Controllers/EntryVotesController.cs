﻿using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.WebAPI.CustomAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryVotesController : ControllerBase
    {
        IEntryVoteService _entryVoteService;

        public EntryVotesController(IEntryVoteService entryVoteService)
        {
            _entryVoteService = entryVoteService;
        }

        [Authorize]
        [RegisterJwtClaimsToItems]
        [HttpGet("{entryId}/vote/up")]
        public IActionResult EntryUpVote(string entryId)
        {
            var result = _entryVoteService.EntryUpVote(entryId);
            
            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}