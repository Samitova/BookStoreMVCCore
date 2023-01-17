using BookStore.DataAccess.Models;
using BookStore.Services.Contracts;
using BookStore.Services.Managers;
using BookStore.Services.ShopService;
using BookStore.Services.ShopService.PaginationService;
using BookStore.Services.ShopService.SearchService;
using BookStore.Services.ShopService.SortingService;
using BookStore.ViewModelData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    [Breadcrumb("Authors")]    
    public class AuthorController : Controller
    {
        private readonly Dictionary<string, string> sortedProperties = new Dictionary<string, string>()
        { { "title", "title" }, { "price", "price_desc" }, { "rating", "rating_desc" },
        { "bestsellers", "bestsellers_desc" }, { "novelties", "novelties_desc" } };

        private string _uploadsFolder;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;
        private readonly IShopManager _shopManager;

        public AuthorController(IShopManager shopManager, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
            _shopManager = shopManager;
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewData["SearchBar"] = new SearchBar() { Action = "Index", Controler = "Author", SearchText = "" };
            _uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "pictures\\uploads\\authors\\");
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<AuthorViewModel> authorsList = await _shopManager.AuthorManager.GetAllAuthorsAsync();
            return View(authorsList);
        }

        [HttpGet]
        [Breadcrumb(Title = "ViewData.Title")]
        public IActionResult CreateAuthor(int? id)
        {
            AuthorViewModel author = new AuthorViewModel();           
            return View(author);           
        }

        [HttpPost]       
        public IActionResult CreateAuthor(AuthorViewModel author)
        {
            if (!ModelState.IsValid)
            {
                return View(author);
            }
            if (author.AuthorImage != null && author.AuthorImage.Length > 0)
            {
                if (_fileService.IsProperImageExtention(author.AuthorImage.ContentType.ToLower()))
                {
                    author.PhotoPath = _fileService.UploadFile(author.AuthorImage, _uploadsFolder);
                }
                else
                {
                    ModelState.AddModelError("", "Bad image extention");
                    return View(author);
                }
            }
            else
            {
                author.PhotoPath = "no_image.png";
            }

            try
            {
                _shopManager.AuthorManager.AddAuthor(author);
                TempData["success"] = "Author was added successfuly";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(author);
            }
        }

        [HttpGet]
        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> UpdateAuthor(int? id)
        {
            AuthorViewModel author = await _shopManager.AuthorManager.GetAuthorByIdAsync(id);
            if (author == null)
                return NotFound();
            else
            {
                return View(author);
            }
        }

        [HttpPost]       
        public IActionResult UpdateAuthor(AuthorViewModel author)
        {
            if (!ModelState.IsValid)
            {
                return View(author);
            }
            if (author.AuthorImage != null && author.AuthorImage.Length > 0)
            {                
                if (_fileService.IsProperImageExtention(author.AuthorImage.ContentType.ToLower()))
                {
                    _fileService.DeleteFile(author.PhotoPath, _uploadsFolder);
                    author.PhotoPath = _fileService.UploadFile(author.AuthorImage, _uploadsFolder);                    
                }
                else
                {
                    ModelState.AddModelError("", "Bad image extention");
                    return View(author);
                }
            }

            try
            {
                _shopManager.AuthorManager.UpdateAuthor(author);
                TempData["success"] = "Author was updated successfuly";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(author);
            }           
        }

        [HttpGet]
        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _shopManager.AuthorManager.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }
            return View(author);
        }

        [HttpPost, ActionName("DeleteAuthor")]
        [ValidateAntiForgeryToken]       
        public async Task<IActionResult> DeleteAuthorPost(int id)
        {
            AuthorViewModel author = await _shopManager.AuthorManager.GetAuthorByIdAsync(id);

            try
            {
                _shopManager.AuthorManager.DeleteAuthor(id);
                _fileService.DeleteFile(author.PhotoPath, _uploadsFolder);
                TempData["success"] = $"Book \"{author.FullName}\" was deleted successfuly";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(author);
            }
        }

        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> AuthorDetails(int id, int PageSize, int CurrentPage = 1, string SortExpression = "")
        {
            NavigationService navigationService = new NavigationService();
            AuthorViewModel author = new AuthorViewModel();

            PageSize = PaginationService.ProccessPageSize(PageSize, this.HttpContext);
            SortModel sortModel = SortService.SetSortModel("AuthorDetails", SortExpression, sortedProperties);
            ViewData["SortModel"] = sortModel;

            if (id == 0)
            {
                author = JsonConvert.DeserializeObject<AuthorViewModel>(HttpContext.Session.GetString("Author"));
                author.Books = SortService.SortBooks(author.Books, sortModel);
            }
            else
            {
                author = await _shopManager.AuthorManager.GetAuthorWithBooksAsync(id, sortModel);
                HttpContext.Session.SetString("Author", JsonConvert.SerializeObject(author));
            }

            navigationService.SetNavigationService("AuthorDetails", author.Books, CurrentPage, PageSize, "", SortExpression);
            ViewData["NavigationService"] = navigationService;
            return View(author);
        }
       
    }
}
