using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Services.Contracts
{
    public interface IFileService
    {
        bool IsProperImageExtention(string fileExtention);
        string UploadFile(IFormFile file, string uploadsFolder);
        void DeleteFile(string uniqueFileName, string uploadsFolder);
    }
}
