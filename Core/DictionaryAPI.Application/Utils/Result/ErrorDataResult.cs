﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.Utils.Result
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        public ErrorDataResult(string message, T data) : base(false, message, data)
        {
            
        }

        public ErrorDataResult(T data) : base(false, data)
        {
            
        }
    }
}
