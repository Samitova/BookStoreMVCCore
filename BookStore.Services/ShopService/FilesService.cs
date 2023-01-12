using BookStore.Services.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace BookStore.Services.ShopService
{
    public  class FilesService: IFileService
    {
        public FilesService()
        {
        }
        public bool IsProperImageExtention(string fileExtention)
        {
            if (fileExtention == "image/jpg" || fileExtention == "image/jpeg" || fileExtention == "image/pjpeg" ||
                fileExtention == "image/gif" || fileExtention == "image/x-png" || fileExtention == "image/png" ||
                fileExtention == "image/webp")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string UploadFile(IFormFile file, string uploadsFolder)
        {
            string uniqueFileName = null;

            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            return uniqueFileName;
        }

        public void DeleteFile(string uniqueFileName, string uploadsFolder)
        {
            if (uniqueFileName != "no_image.png")
            {
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }
    }
}
