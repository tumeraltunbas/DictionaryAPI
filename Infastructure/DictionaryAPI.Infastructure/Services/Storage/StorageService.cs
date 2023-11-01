using Azure.Core;
using DictionaryAPI.Application.Abstracts.Services.StorageService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryAPI.Infastructure.Services.Storage
{
    public class StorageService : IStorageService
    {
        public string UploadFile(IFormFile file, string uploadPath)
        {

            //Mimetype check will be added.

            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            Random r = new();

            string randomFileName = $"{r.Next()}{Path.GetExtension(file.FileName)}";

            string filePath = Path.Combine(uploadPath, $"{randomFileName}");

            FileStream fileStream = new(
                path: filePath,
                mode: FileMode.Create,
                access: FileAccess.Write,
                share: FileShare.None
                );

            file.CopyTo(fileStream);
            fileStream.Flush();

            return randomFileName;

        }
    }
}
