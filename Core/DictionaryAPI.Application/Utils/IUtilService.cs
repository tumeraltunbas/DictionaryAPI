using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.Utils
{
    public interface IUtilService
    {
        string GenerateRandomString(int size);
        string GenerateEmailVerificationLink();
    }
}
