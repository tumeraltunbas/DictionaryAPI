﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.Utils.Result
{
    public class DataResult<T> : Result
    {
        public T Data { get; }

        public DataResult(bool success, string message, T data) : base(success, message)
        {
            Data = data;
        }
        public DataResult(bool success, T data) : base(success)
        {
            Data = data;
        }

    }
}