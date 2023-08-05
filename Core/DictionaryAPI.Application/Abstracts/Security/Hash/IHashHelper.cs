using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.Abstracts.Security.Hash
{
    public interface IHashHelper
    {
        Tuple<byte[], byte[]> GenerateHash(string password);
    }
}
