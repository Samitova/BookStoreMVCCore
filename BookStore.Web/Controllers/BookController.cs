using BookStore.DataAccess.Models;
using BookStore.Services.Contracts;
using BookStore.Services.ShopService;
using BookStore.Services.ShopService.PaginationService;
using BookStore.Services.ShopService.SearchService;
using BookStore.Services.ShopService.SortingService;
using BookStore.ViewModelData;
using BookStore.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{    
    [DefaultBreadcrumb("Home")]   
    public class BookController : Controller
    {
        private readonly Dictionary<string, string> sortedProperties = new Dictionary<string, string>()
        { { "title", "title" }, { "price", "price_desc" }, { "author", "author" }, { "rating", "rating_desc" },
        { "bestsellers", "bestsellers_desc" }, { "novelties", "novelties_desc" } };

        private readonly IShopManager _shopManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IFileService _fileService;
        private readonly ILogger<AdministrationController> _logger;
        private readonly IDataProtector _dataProtector;
        private string _uploadsFolder;

        public BookController(IShopManager shopManager, IWebHostEnvironment webHostEnvironment, 
                              IFileService fileService, IDataProtectionProvider dataProtectionProvider,
                              DataProtectionPurposeStrings dataProtectionPurposeStrings, 
                              ILogger<AdministrationController> logger)
        {
            _shopManager = shopManager;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
            _logger = logger;
            _dataProtector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.BookIdRouteValue);
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewData["SearchBar"] = new SearchBar() { Action = "Index", Controler = "Book", SearchText = "" };
            _uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "pictures\\uploads\\books\\");

        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);           
            SetCategories();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(int PageSize, string SortExpression = "", string SearchText = "", int CurrentPage = 1)
        {            
            IEnumerable<BookViewModel> books = new List<BookViewModel>();
            NavigationService navigationService = new NavigationService();
            string oldSearchText = "";
            if (SearchText == null)
                SearchText = "";

            ViewData["SearchBar"] = new SearchBar() { Action = "Index", Controler = "Book", SearchText = SearchText };

            // Set custom page size
            PageSize = PaginationService.ProccessPageSize(PageSize, this.HttpContext);

            // set sortModel
            SortModel sortModel = SortService.SetSortModel("Index", SortExpression, sortedProperties);            
            ViewData["SortModel"] = sortModel;

            HttpContext.Session.Remove("CategoryId");

            bool isBooksInSession = HttpContext.Session.TryGetValue("Books", out _);

            if (isBooksInSession)
            {
                oldSearchText = HttpContext.Session.GetString("SearchText");
            }

            if (!isBooksInSession || isBooksInSession && oldSearchText != SearchText || isBooksInSession && SearchText == "")
            {
                books =await _shopManager.BookManager.GetAllBooksAsync(SearchText);
                books = SortService.SortBooks(books, sortModel).Select(e=>
                {
                    e.EncryptedId = _dataProtector.Protect(e.Id.ToString());
                    e.AuthorEncryptedId = _dataProtector.Protect(e.AuthorId.ToString());
                    return e;
                }); 
                
                HttpContext.Session.SetString("Books", JsonConvert.SerializeObject(books));
                HttpContext.Session.SetString("SearchText", SearchText);
            }
            else
            {
                books = JsonConvert.DeserializeObject<PaginatedList<BookViewModel>>(HttpContext.Session.GetString("Books"));
                books = SortService.SortBooks(books, sortModel);
            }

            navigationService.SetNavigationService("Index", books, CurrentPage, PageSize, SearchText, SortExpression);

            ViewData["NavigationService"] = navigationService;
            return View(navigationService.PagedBooks);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> BrowseCategory(string categoryId, int PageSize, string SortExpression = "", int CurrentPage = 1)
        {
            IEnumerable<BookViewModel> books = new List<BookViewModel>();
            NavigationService navigationService = new NavigationService();
            CategoryViewModel categoryVM = new CategoryViewModel();

            PageSize = PaginationService.ProccessPageSize(PageSize, this.HttpContext);

            SortModel sortModel = SortService.SetSortModel("BrowseCategory", SortExpression, sortedProperties);
            ViewData["SortModel"] = sortModel;

            int decryptedCategoryId = Convert.ToInt32(_dataProtector.Unprotect(categoryId));
            if (decryptedCategoryId == 0)
            {
                decryptedCategoryId = (int)HttpContext.Session.GetInt32("CategoryId");
            }

            categoryVM.Categories =  _shopManager.CategoryManager.GetSubCategories(decryptedCategoryId);
            categoryVM.Category = _shopManager.CategoryManager.GetCategoryById(decryptedCategoryId);
            HttpContext.Session.SetInt32("CategoryId", decryptedCategoryId);

            books = await _shopManager.BookManager.GetAllBooksAsync(categoryId: decryptedCategoryId);
            books = SortService.SortBooks(books, sortModel).Select(e =>
            {
                e.EncryptedId = _dataProtector.Protect(e.Id.ToString());
                e.AuthorEncryptedId = _dataProtector.Protect(e.AuthorId.ToString());
                return e;
            });

            var categoryBreadcrumbNode = new MvcBreadcrumbNode("BrowseCategory", "Book", categoryVM.Category.CategoryName);
            BuildBreadcrumbNodeCategoteryTree(categoryVM.Category, categoryBreadcrumbNode);

            navigationService.SetNavigationService("BrowseCategory", books, CurrentPage, PageSize, "", SortExpression);
            ViewData["NavigationService"] = navigationService;
            return View("Index", navigationService.PagedBooks);
        }

        [HttpGet]
        [AllowAnonymous]
        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> BookDetails(string id)
        {     
            int decryptedId = Convert.ToInt32(_dataProtector.Unprotect(id));
            BookViewModel book = await _shopManager.BookManager.GetBookByIdAsync(decryptedId);
            if (book == null)
            {
                Response.StatusCode = 404;
                return View("BookNotFound", decryptedId);
            }
            book.EncryptedId = id;
            return View(book);
        }

        [AllowAnonymous]
        public async Task<IActionResult> AddBookComment(BookComment bookComment)
        {
            if (string.IsNullOrEmpty(bookComment.PublisherName))
                bookComment.PublisherName = "Anonimus";
            await _shopManager.BookManager.AddBookComment(bookComment);
            BookViewModel book = await _shopManager.BookManager.GetBookByIdAsync(bookComment.BookId);
            var breadcrumbNode = new MvcBreadcrumbNode("BrowseCategory", "Book", book.Title);
            ViewData["BreadcrumbNode"] = breadcrumbNode;           
            return View("BookDetails", book);
        }

        //******************************************Admin functions**********************************

        #region AdminFunction
        [HttpGet]
        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> CreateBook(int? id)
        {
            BookViewModel book = new BookViewModel();
            await SetBookFields(book);
            return View(book);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook(BookViewModel book)
        {
            if (!ModelState.IsValid)
            {
                await SetBookFields(book);
                return View(book);
            }
            if (book.BookImage != null && book.BookImage.Length > 0)
            {
                if (_fileService.IsProperImageExtention(book.BookImage.ContentType.ToLower()))
                {
                    book.PhotoPath = _fileService.UploadFile(book.BookImage, _uploadsFolder);
                }
                else
                {
                    ModelState.AddModelError("", "Bad image extention");
                    return View(book);
                }
            }
            else
            {
                book.PhotoPath = "no_image.png";
            }

            book.CategoryName = _shopManager.CategoryManager.GetCategoryById(book.CategoryId)?.CategoryName;
            book.AuthorFullName = _shopManager.AuthorManager.GetAuthorById(book.AuthorId)?.FullName;
            book.PublisherName = _shopManager.PublisherManager.GetPublisherById(book.PublisherId)?.PublisherName;

            try
            {
                _shopManager.BookManager.AddBook(book);
                TempData["success"] = "Book was added successfuly";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(book);
            }           
        }

        [HttpGet]
        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> EditBook(string id)
        {
            int decryptedId = Convert.ToInt32(_dataProtector.Unprotect(id));
            BookViewModel book = new BookViewModel();
            book = await _shopManager.BookManager.GetBookByIdAsync(decryptedId);
            book.EncryptedId = id;
            if (book == null)
                return NotFound();
            else
            {
                await SetBookFields(book);
                return View(book);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditBook(BookViewModel book)
        {           
            book.Id = _dataProtector.Unprotect(book.EncryptedId);
            if (!ModelState.IsValid)
            {
                await SetBookFields(book);                
                return View(book);
            }
            if (book.BookImage != null && book.BookImage.Length > 0)
            {
                if (_fileService.IsProperImageExtention(book.BookImage.ContentType.ToLower()))
                {
                    _fileService.DeleteFile(book.PhotoPath, _uploadsFolder);
                    book.PhotoPath = _fileService.UploadFile(book.BookImage, _uploadsFolder);
                }
                else
                {
                    ModelState.AddModelError("", "Bad image extention");
                    return View(book);
                }
            }

            book.CategoryName = _shopManager.CategoryManager.GetCategoryById(book.CategoryId)?.CategoryName;
            book.AuthorFullName = _shopManager.AuthorManager.GetAuthorById(book.AuthorId)?.FullName;
            book.PublisherName = _shopManager.PublisherManager.GetPublisherById(book.PublisherId)?.PublisherName;

            try
            {             
                _shopManager.BookManager.UpdateBook(book);
                TempData["success"] = "Book was updated successfuly";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("", ex.Message);
                return View(book);
            }
        }

        [HttpGet]
        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> DeleteBook(string id)
        {
            int decryptedId = Convert.ToInt32(_dataProtector.Unprotect(id));
            var book = await _shopManager.BookManager.GetBookByIdAsync(decryptedId);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost, ActionName("DeleteBook")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBookPost(string id)
        {
            int decryptedId = Convert.ToInt32(_dataProtector.Unprotect(id));
            var book = await _shopManager.BookManager.GetBookByIdAsync(decryptedId);
            if (book == null)
            {
                return NotFound();
            }

            try
            {
                _shopManager.BookManager.DeleteBook(decryptedId);
                _fileService.DeleteFile(book.PhotoPath, _uploadsFolder);
                TempData["success"] = $"Book \"{book.Title}\" was deleted successfuly";
                return RedirectToAction("Index");
            }          
            catch (DbUpdateException ex)
            {                
                _logger.LogError($"Error deleting book {book.Title}. {ex}");
                ViewBag.ErrorTitle = $"{book.Title} book is in use";
                ViewBag.ErrorMessage = $"{book.Title} book cannot be deleted as there are users in this role" +
                    $"If you want to delete this book, please remove the users from the role and try again";
                return View("Error");
            } 
        }

        #endregion

        //*******************************************************************************************

        #region PrivateFunction
        private void SetCategories()
        {
            CategoryViewModel categoryVM = new CategoryViewModel();
            bool isCategoryIdExists = HttpContext.Session.TryGetValue("CategoryId", out byte[] _);
            if (!isCategoryIdExists) 
            {
                categoryVM.Categories = _shopManager.CategoryManager.GetSubCategories(0).Select(e =>
                {
                    e.EncryptedId = _dataProtector.Protect(e.Id.ToString());
                    return e;
                }); ;
            }          
            else 
            {
                int id = (int)HttpContext.Session.GetInt32("CategoryId");
                categoryVM.Categories = _shopManager.CategoryManager.GetSubCategories(id).Select(e =>
                {
                    e.EncryptedId = _dataProtector.Protect(e.Id.ToString());
                    return e;
                }); ;

                categoryVM.Category = _shopManager.CategoryManager.GetCategoryById(id);
                categoryVM.Category.EncryptedId = _dataProtector.Protect(categoryVM.Category.Id.ToString());
            }
            ViewData["Categories"] = categoryVM;           
        }
        private void BuildBreadcrumbNodeCategoteryTree(Category category, MvcBreadcrumbNode node)
        {
            if (category.ParentId == 0)
            {
                var parentCategoryBreadcrumbNode = new MvcBreadcrumbNode("Index", "Book", "Books");
                node.Parent = parentCategoryBreadcrumbNode;
            }
            else
            {
                var parentCategory = _shopManager.CategoryManager.GetCategoryById(category.ParentId);
                var parentCategoryBreadcrumbNode = new MvcBreadcrumbNode("BrowseCategory", "Book", parentCategory.CategoryName);
                parentCategoryBreadcrumbNode.RouteValues = new { categoryId = _dataProtector.Protect(parentCategory.Id.ToString())};
                node.Parent = parentCategoryBreadcrumbNode;
                BuildBreadcrumbNodeCategoteryTree(parentCategory, parentCategoryBreadcrumbNode);
            }

            ViewData["BreadcrumbNode"] = node;           
        }
        private async Task SetBookFields(BookViewModel book)
        {
            book.Categories = (await _shopManager.CategoryManager.GetAllCategoriesAsync()).Select(i => new SelectListItem()
            {
                Text = i.CategoryName,
                Value = i.Id.ToString()
            });

            book.Authors = (await _shopManager.AuthorManager.GetAllAuthorsAsync()).Select(i => new SelectListItem()
            {
                Text = i.FullName,
                Value = i.Id.ToString()
            });

            book.Publishers = (await _shopManager.PublisherManager.GetAllPublishersAsync()).Select(i => new SelectListItem()
            {
                Text = i.PublisherName,
                Value = i.Id.ToString()
            });
        }       

        #endregion
    }
}
