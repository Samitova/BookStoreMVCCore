using BookStore.DataAccess.Models;
using BookStore.Services.Contracts;
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
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SmartBreadcrumbs.Attributes;
using SmartBreadcrumbs.Nodes;
using System;
using System.Collections.Generic;
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
        private string _uploadsFolder;

        public BookController(IShopManager shopManager, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
            _shopManager = shopManager;
            _webHostEnvironment = webHostEnvironment;
            _fileService = fileService;
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
                books = SortService.SortBooks(books, sortModel);               
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
        public async Task<IActionResult> BrowseCategory(int categoryId, int PageSize, string SortExpression = "", int CurrentPage = 1)
        {
            IEnumerable<BookViewModel> books = new List<BookViewModel>();
            NavigationService navigationService = new NavigationService();
            CategoryViewModel categoryVM = new CategoryViewModel();

            PageSize = PaginationService.ProccessPageSize(PageSize, this.HttpContext);

            SortModel sortModel = SortService.SetSortModel("BrowseCategory", SortExpression, sortedProperties);
            ViewData["SortModel"] = sortModel;

            if (categoryId == 0)
            {
                categoryId = (int)HttpContext.Session.GetInt32("CategoryId");
            }

            categoryVM.Categories =  _shopManager.CategoryManager.GetSubCategories(categoryId);
            categoryVM.Category = _shopManager.CategoryManager.GetCategoryById(categoryId);
            HttpContext.Session.SetInt32("CategoryId", categoryId);

            books = await _shopManager.BookManager.GetAllBooksAsync(categoryId: categoryId);
            books = SortService.SortBooks(books, sortModel);

            var categoryBreadcrumbNode = new MvcBreadcrumbNode("BrowseCategory", "Book", categoryVM.Category.CategoryName);
            BuildBreadcrumbNodeCategoteryTree(categoryVM.Category, categoryBreadcrumbNode);

            navigationService.SetNavigationService("BrowseCategory", books, CurrentPage, PageSize, "", SortExpression);
            ViewData["NavigationService"] = navigationService;
            return View("Index", navigationService.PagedBooks);
        }

        [HttpGet]
        [AllowAnonymous]
        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> BookDetails(int id)
        {            
            BookViewModel book = await _shopManager.BookManager.GetBookByIdAsync(id);
            if (book == null)
            {
                Response.StatusCode = 404;
                return View("BookNotFound", id);
            }
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
        public async Task<IActionResult> UpdateBook(int id)
        {
            BookViewModel book = new BookViewModel();
            book = await _shopManager.BookManager.GetBookByIdAsync(id);
            if (book == null)
                return NotFound();
            else
            {
                await SetBookFields(book);
                return View(book);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBook(BookViewModel book)
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
        public async Task<IActionResult> DeleteBook(int id)
        {           
            var book = await _shopManager.BookManager.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost, ActionName("DeleteBook")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBookPost(int id)
        {
            var book = await _shopManager.BookManager.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            try
            {
                _shopManager.BookManager.DeleteBook(id);
                _fileService.DeleteFile(book.PhotoPath, _uploadsFolder);
                TempData["success"] = $"Book \"{book.Title}\" was deleted successfuly";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(book);
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
                categoryVM.Categories = _shopManager.CategoryManager.GetSubCategories(0);
            }          
            else 
            {
                int id = (int)HttpContext.Session.GetInt32("CategoryId");
                categoryVM.Categories = _shopManager.CategoryManager.GetSubCategories(id);

                categoryVM.Category = _shopManager.CategoryManager.GetCategoryById(id);
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
                parentCategoryBreadcrumbNode.RouteValues = new { categoryId = parentCategory.Id };
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
