﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.DTO.DTOs
{
    public class ResetPasswordDto
    {
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
    }
}
