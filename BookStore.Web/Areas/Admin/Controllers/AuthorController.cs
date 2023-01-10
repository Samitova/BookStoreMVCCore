using AutoMapper;
using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.Models;
using BookStore.Services.Contracts;
using BookStore.ViewModelData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BookStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;        
        private readonly IAuthorManager _authorManager;        

        public AuthorController(IAuthorManager authorManager, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _authorManager = authorManager;    
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<AuthorVM> authorsList = await _authorManager.GetAllAuthorsAsync();            
            return View(authorsList);
        }

        [HttpGet]
        public async Task<IActionResult> CreateUpdateAuthor(int? id)
        {
            AuthorVM author = new AuthorVM();
            if (id == null || id == 0)
            {
                return View(author);
            }
            else
            {
                author = await _authorManager.GetAuthorByIdAsync(id);
                if (author == null)
                    return NotFound();
                else
                {
                    return View(author);
                }
            }
        }

        [HttpPost]
        public IActionResult CreateUpdateAuthor(AuthorVM author)
        {
            if (!ModelState.IsValid)
            {               
                return View(author);
            }
            if (author.AuthorImage != null && author.AuthorImage.Length > 0)
            {
                string ext = author.AuthorImage.ContentType.ToLower();
                if (ext != "image/jpg" && ext != "image/jpeg" && ext != "image/pjpeg" &&
                    ext != "image/gif" && ext != "image/x-png" && ext != "image/png" && ext != "image/webp")
                {
                    ModelState.AddModelError("", "Bad image extention");                    
                    return View(author);
                }
                else
                {
                    string uniqueFileName = UploadFile(author);
                    author.PhotoPath = uniqueFileName;
                }
            }
            else
            {
                if(author.Id==0)
                    author.PhotoPath = "no_image.png";
            }

            if (author.Id == 0)
            {
                _authorManager.AddAuthor(author);
                TempData["success"] = "Author was added successfuly";
            }
            else
            {
                _authorManager.UpdateAuthor(author);
                TempData["success"] = "Author was updated successfuly";               
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task <IActionResult> DeleteAuthor(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var author = await _authorManager.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        [HttpPost, ActionName("DeleteAuthor")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAuthorPost(int? id)
        {  
            var author = _authorManager.DeleteAuthor (id);
            DeleteFile(author.PhotoPath);

            TempData["success"] = $"Book \"{author.FullName}\" was deleted successfuly";
            return RedirectToAction("Index");
        }

        private string UploadFile(AuthorVM author)
        {
            string uniqueFileName = null;
            if (author.AuthorImage != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "pictures\\uploads\\authors\\");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + author.AuthorImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    author.AuthorImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

        private void DeleteFile(string uniqueFileName)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "pictures\\uploads\\authors\\");
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
