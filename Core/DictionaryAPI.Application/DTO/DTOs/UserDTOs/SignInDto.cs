using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.DTO.DTOs.UserDTOs
{
    public class SignInDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
