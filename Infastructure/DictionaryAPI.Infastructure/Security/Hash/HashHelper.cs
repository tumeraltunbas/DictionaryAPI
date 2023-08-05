using DictionaryAPI.Application.Abstracts.Security.Hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Infastructure.Security.Hash
{
    public class HashHelper : IHashHelper
    {
        public Tuple<byte[], byte[]> GenerateHash(string password)
        {
            using (HMACSHA256 algorithm = new())
            {
                byte[] salt = algorithm.Key;
                byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(password));

                return new Tuple<byte[], byte[]>(salt, hash);
            }

        }
    }
}
