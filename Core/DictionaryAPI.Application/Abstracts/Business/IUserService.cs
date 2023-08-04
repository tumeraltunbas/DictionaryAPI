using DictionaryAPI.Application.DTO.DTOs;
using DictionaryAPI.Application.Utils.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.Abstracts.Business
{
    public interface IUserService
    {
        Result SignUp(SignUpDto signUpDto);
    }
}
