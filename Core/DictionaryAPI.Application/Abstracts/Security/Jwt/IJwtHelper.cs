using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DictionaryAPI.Domain.Entities;

namespace DictionaryAPI.Application.Abstracts.Security.JWT
{
    public interface IJwtHelper
    {
        string GenerateJwt(User user);
    }
}
