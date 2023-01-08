using AutoMapper;
using BookStore.Data.Models.ModelsDTO;
using BookStore.Data.Models.ViewModels;
using BookStore.Services.DataBaseService.Interfaces;
using BookStore.Services.ShopService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BookStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthorController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;

        public AuthorController(IRepositoryWrapper repositoryWrapper, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _webHostEnvironment = webHostEnvironment;
            _repository = repositoryWrapper;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            List<AuthorDTO> authors = _repository.Authors.GetAll(orderBy: x => x.OrderBy(y => y.FullName)).ToList();
            List<AuthorVM> model = _mapper.Map<IEnumerable<AuthorVM>>(authors).ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateUpdateAuthor(int? id)
        {
            AuthorDTO author = new AuthorDTO();
            if (id == null || id == 0)
            {
                return View(_mapper.Map<AuthorVM>(author));
            }
            else
            {
                author = _repository.Authors.GetById(id);
                if (author == null)
                    return NotFound();
                else
                {
                    return View(_mapper.Map<AuthorVM>(author));
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

            AuthorDTO authorDTO = _mapper.Map<AuthorDTO>(author);

            if (author.Id == 0)
            {
                _repository.Authors.Add(authorDTO);
                TempData["success"] = "Author was added successfuly";
            }
            else
            {
                _repository.Authors.Update(authorDTO);
                TempData["success"] = "Author was updated successfuly";               
            }
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
    }
}
