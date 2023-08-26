using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.WebAPI.CustomAttributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryFavoritesController : ControllerBase
    {
        IEntryFavoriteService _entryFavoriteDal;

        public EntryFavoritesController(IEntryFavoriteService entryFavoriteDal)
        {
            _entryFavoriteDal = entryFavoriteDal;
        }

        [Authorize]
        [RegisterJwtClaimsToItems]
        [HttpGet("{entryId}/favorite")]
        public IActionResult FavoriteEntry(string entryId)
        {
            var result = _entryFavoriteDal.FavoriteEntry(entryId);

            if(result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
