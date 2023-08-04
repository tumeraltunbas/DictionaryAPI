using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.Utils.Result
{
    public class ErrorResult : Result
    {
        public ErrorResult() : base(false)
        {
            
        }

        public ErrorResult(string message) : base(false, message)
        {

        }
    }
}
