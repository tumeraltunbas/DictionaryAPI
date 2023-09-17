using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.Utils.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TitlesController : ControllerBase
    {
        ITitleService _titleService;
        public TitlesController(ITitleService titleService)
        {
            _titleService = titleService;
        }

        [HttpGet("{titleSlug}")]
        public IActionResult GetTitleBySlug(string titleSlug)
        {
            var result = _titleService.GetTitleBySlug(titleSlug);

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("random/{titleCount}")]
        public IActionResult GetRandomTitles(int titleCount)
        {
            var result = _titleService.GetRandomTitles(titleCount);

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
