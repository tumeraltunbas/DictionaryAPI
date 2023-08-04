using DictionaryAPI.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.Abstracts.Business
{
    public interface IUserService
    {
        void SignUp(SignUpDto signUpDto);
    }
}
