using DictionaryAPI.Application.Abstracts.Business;
using DictionaryAPI.Application.DTO.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryAPI.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class UsersController : ControllerBase
    {
        IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("sign/up")]
        public IActionResult SignUp(SignUpDto signUpDto)
        {
            var result = _userService.SignUp(signUpDto);
            
            if (result.Success != true)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }


    }
}
