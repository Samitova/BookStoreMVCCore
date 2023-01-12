using BookStore.Services.Contracts;
using BookStore.Services.Managers;
using BookStore.Services.ShopService;
using BookStore.Services.ShopService.PaginationService;
using BookStore.Services.ShopService.SearchService;
using BookStore.Services.ShopService.SortingService;
using BookStore.ViewModelData;
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

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IShopManager _shopManager;

        public AuthorController(IShopManager shopManager, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            _shopManager = shopManager;
        }
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewData["SearchBar"] = new SearchBar() { Action = "Index", Controler = "Author", SearchText = "" };           
        }
                
        public async Task<IActionResult> Index()
        {
            IEnumerable<AuthorVM> authorsList = await _shopManager.AuthorManager.GetAllAuthorsAsync();
            return View(authorsList);
        }

        [HttpGet]
        [Breadcrumb(Title = "ViewData.Title")]
        public IActionResult CreateAuthor(int? id)
        {
            AuthorVM author = new AuthorVM();           
            return View(author);           
        }

        [HttpPost]       
        public IActionResult CreateAuthor(AuthorVM author)
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
                author.PhotoPath = "no_image.png";
            }
           
            _shopManager.AuthorManager.AddAuthor(author);
            TempData["success"] = "Author was added successfuly";
          
            return RedirectToAction("Index");
        }


        [HttpGet]
        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> UpdateAuthor(int? id)
        {
            AuthorVM author = new AuthorVM();
            if (id == null || id == 0)
            {
                return View(author);
            }
            else
            {
                author = await _shopManager.AuthorManager.GetAuthorByIdAsync(id);
                if (author == null)
                    return NotFound();
                else
                {
                    return View(author);
                }
            }
        }

        [HttpPost]       
        public IActionResult UpdateAuthor(AuthorVM author)
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
                if (author.Id == 0)
                    author.PhotoPath = "no_image.png";
            }

            if (author.Id == 0)
            {
                _shopManager.AuthorManager.AddAuthor(author);
                TempData["success"] = "Author was added successfuly";
            }
            else
            {
                _shopManager.AuthorManager.UpdateAuthor(author);
                TempData["success"] = "Author was updated successfuly";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
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
            AuthorVM author = await _shopManager.AuthorManager.GetAuthorByIdAsync(id);
            _shopManager.AuthorManager.DeleteAuthor(id);
            DeleteFile(author.PhotoPath);

            TempData["success"] = $"Book \"{author.FullName}\" was deleted successfuly";
            return RedirectToAction("Index");
        }


        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> AuthorDetails(int id, int PageSize, int CurrentPage = 1, string SortExpression = "")
        {
            NavigationService navigationService = new NavigationService();
            AuthorVM author = new AuthorVM();

            PageSize = PaginationService.ProccessPageSize(PageSize, this.HttpContext);
            SortModel sortModel = SortService.SetSortModel("AuthorDetails", SortExpression, sortedProperties);
            ViewData["SortModel"] = sortModel;

            if (id == 0)
            {
                author = JsonConvert.DeserializeObject<AuthorVM>(HttpContext.Session.GetString("Author"));
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
