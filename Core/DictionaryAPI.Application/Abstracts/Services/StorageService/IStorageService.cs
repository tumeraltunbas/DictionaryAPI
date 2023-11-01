using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Application.Abstracts.Services.StorageService
{
    public interface IStorageService
    {        
        /*
         Modification List:
         I must upload both single file and multiple files and I must restrict this when I call the function.
         I must send the allowed file types as a parameter.
         */

        string UploadFile(IFormFile file, string uploadPath);
    }
}
