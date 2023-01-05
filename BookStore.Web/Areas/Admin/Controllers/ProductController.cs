using BookStore.Data.Models.ModelsDTO;
using BookStore.Data.Models.ViewModels;
using BookStore.Services.DataBaseService.Interfaces;
using BookStore.Services.ShopService.PaginationService;
using BookStore.Services.ShopService;
using BookStore.Services.ShopService.SearchService;
using BookStore.Services.ShopService.SortingService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBreadcrumbs.Nodes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;
using static System.Net.WebRequestMethods;
using AutoMapper;

namespace BookStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]   
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        private readonly ShopService _bookService;
        private SortModel _sortModel = new SortModel();
        private int _pageSize = 4;
        
        public ProductController(IRepositoryWrapper repositoryWrapper, ShopService bookService, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            _bookService = bookService;
            _repository = repositoryWrapper;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);           
            ViewData["SearchBar"] = new SearchBar() { Action = "Index", Controler = "Product", SearchText = "" };
            SetSortModel();
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            SetCategories();
        }
        
        public IActionResult Index(int PageSize, string SortExpression = "", string SearchText = "", int CurrentPage = 1)
        {
            List<BookVM> books = new List<BookVM>();
            NavigationService navigationService = new NavigationService();
            string oldSearchText = "";
            if (SearchText == null)
                SearchText = "";

            ViewData["SearchBar"] = new SearchBar() { Action = "Index", Controler = "Product", SearchText = SearchText };


            PageSize = ProccessPageSize(PageSize);

            _sortModel.ApplySort("Index", SortExpression);
            ViewData["SortModel"] = _sortModel;

            HttpContext.Session.Remove("CategoryId");

            bool isBooksInSession = HttpContext.Session.TryGetValue("Books", out _);

            if (isBooksInSession)
            {
                oldSearchText = HttpContext.Session.GetString("SearchText");
            }

            if (!isBooksInSession || isBooksInSession && oldSearchText != SearchText || isBooksInSession && SearchText == "")
            {
                books = _bookService.GetAllBooksFromDb(SearchText);
                books = _bookService.DoSort(books, _sortModel.SortedProperty, _sortModel.SortedOrder);
                HttpContext.Session.SetString("Books", JsonConvert.SerializeObject(books));
                HttpContext.Session.SetString("SearchText", SearchText);
            }
            else
            {
                books = JsonConvert.DeserializeObject<PaginatedList<BookVM>>(HttpContext.Session.GetString("Books"));
                books = _bookService.DoSort(books, _sortModel.SortedProperty, _sortModel.SortedOrder);
            }

            navigationService.SetNavigationService("Index", books, CurrentPage, PageSize, SearchText, SortExpression);

            ViewData["NavigationService"] = navigationService;
            return View(navigationService.PagedBooks);
        }

        public IActionResult Category(int categoryId, int PageSize, string SortExpression = "", int CurrentPage = 1)
        {
            List<BookVM> books = new List<BookVM>();
            NavigationService navigationService = new NavigationService();
            CategoryVM categoryVM = new CategoryVM();

            PageSize = ProccessPageSize(PageSize);

            _sortModel.ApplySort("Category", SortExpression);
            ViewData["SortModel"] = _sortModel;

            if (categoryId == 0)
            {
                categoryId = (int)HttpContext.Session.GetInt32("CategoryId");
            }

            categoryVM.Categories = _repository.Categories.GetAll(filter: x => x.ParentId == categoryId, orderBy: x => x.OrderBy(y => y.CategoryName)).ToList();
            categoryVM.Category = _repository.Categories.GetById(categoryId);
            HttpContext.Session.SetInt32("CategoryId", categoryId);

            books = _bookService.GetAllBooksFromDb(categoryId: categoryId);
            books = _bookService.DoSort(books, _sortModel.SortedProperty, _sortModel.SortedOrder);

            var categoryBreadcrumbNode = new MvcBreadcrumbNode("Category", "Product", categoryVM.Category.CategoryName);
            BuildBreadcrumbNodeCategoteryTree(categoryVM.Category, categoryBreadcrumbNode);

            navigationService.SetNavigationService("Category", books, CurrentPage, PageSize, "", SortExpression);
            ViewData["NavigationService"] = navigationService;
            return View("Index", navigationService.PagedBooks);
        }

        [HttpGet]
        public IActionResult CreateProduct(int id)
        {
            BookVM book = new BookVM();
            SetBookFields(book);
            return View(book);
        }

        [HttpPost]
        public IActionResult CreateProduct(BookVM book)
        {
            if (!ModelState.IsValid)
            {
                SetBookFields(book);
                return View(book);
            }
            if (book.BookImage != null && book.BookImage.Length > 0)
            {
                string ext = book.BookImage.ContentType.ToLower();

                if (ext != "image/jpg" && ext != "image/jpeg" && ext != "image/pjpeg" &&
                    ext != "image/gif" && ext != "image/x-png" && ext != "image/png" && ext != "image/webp")
                {
                    ModelState.AddModelError("", "Bad image extention");
                    SetBookFields(book);
                    return View(book);
                }
                else
                {
                    string uniqueFileName = UploadFile(book);
                    book.PhotoPath = uniqueFileName;
                }
            }
            else 
            {
                book.PhotoPath = "no_image.png";
            }

            book.CategoryName = _repository.Categories.GetById(book.CategoryId)?.CategoryName;
            book.AuthorFullName = _repository.Authors.GetById(book.AuthorId)?.FullName;
            book.PublisherName = _repository.Publishers.GetById(book.PublisherId)?.PublisherName;

            BookDTO bookDTO = _mapper.Map<BookDTO>(book);
            _repository.Books.Add(bookDTO);
            TempData["success"] = "Book was added successfuly";          
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult UpdateProduct(int id)
        {
            BookVM book = new BookVM();
            book = _bookService.GetBookById(id);
            if (book == null)
                return NotFound();
            else
            {
                SetBookFields(book);
                return View(book);
            }
        }

        [HttpPost]
        public IActionResult UpdateProduct(BookVM book)
        {
            if (!ModelState.IsValid)
            {
                SetBookFields(book);
                return View(book);
            }
            if (book.BookImage != null && book.BookImage.Length > 0)
            {
                string ext = book.BookImage.ContentType.ToLower();

                if (ext != "image/jpg" && ext != "image/jpeg" && ext != "image/pjpeg" &&
                    ext != "image/gif" && ext != "image/x-png" && ext != "image/png" && ext != "image/webp")
                {
                    ModelState.AddModelError("", "Bad image extention");
                    SetBookFields(book);
                    return View(book);
                }
                else
                {
                    string uniqueFileName = UploadFile(book);
                    book.PhotoPath = uniqueFileName;
                }
            }

            book.CategoryName = _repository.Categories.GetById(book.CategoryId)?.CategoryName;
            book.AuthorFullName = _repository.Authors.GetById(book.AuthorId)?.FullName;
            book.PublisherName = _repository.Publishers.GetById(book.PublisherId)?.PublisherName;
            BookDTO bookDTO = _mapper.Map<BookDTO>(book);

             _repository.Books.Update(bookDTO);
            TempData["success"] = "Book was updated successfuly";           

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult BookDetails(int id)
        {
            BookVM book = _bookService.GetBookById(id);
            return View(book);        }

        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var book = _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProductPost(int? id)
        {
            var book = _repository.Books.GetById(id);
            if (book == null)
            {
                return NotFound();
            }            
            
            _repository.Books.Delete(book);
            DeleteFile(book.PhotoPath);

            TempData["success"] = @$"Book ""{book.Title }"" was deleted successfuly";
            return RedirectToAction("Index");
        }

        #region privateFunctions
        private void SetBookFields(BookVM book) 
        {
            book.Categories = _repository.Categories.GetAll(orderBy: x => x.OrderBy(y => y.CategoryName)).Select(i => new SelectListItem()
            {
                Text = i.CategoryName,
                Value = i.Id.ToString()
            });

            book.Authors = _repository.Authors.GetAll(orderBy: x => x.OrderBy(y => y.FullName)).Select(i => new SelectListItem()
            {
                Text = i.FullName,
                Value = i.Id.ToString()
            });

            book.Publishers = _repository.Publishers.GetAll(orderBy: x => x.OrderBy(y => y.PublisherName)).Select(i => new SelectListItem()
            {
                Text = i.PublisherName,
                Value = i.Id.ToString()
            });
        }
        private string UploadFile(BookVM book) 
        {
            string uniqueFileName = null;
            if (book.BookImage != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "pictures\\uploads\\books\\");
                uniqueFileName = Guid.NewGuid().ToString()+"_"+book.BookImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    book.BookImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
        private void DeleteFile(string uniqueFileName)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "pictures\\uploads\\books\\");
            if (uniqueFileName != "no_image.png") 
            {
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }  
        }
        private void SetSortModel()
        {
            Dictionary<string, string> sortedProperties = new Dictionary<string, string>() { { "title", "title" }, { "price", "price_desc" }, { "author", "author" }, { "rating", "rating_desc" } };
            _sortModel.InitSortModel(sortedProperties);
            _sortModel.ApplySort("Index", "");
            ViewData["SortModel"] = _sortModel;
        }
        private void SetCategories()
        {
            CategoryVM categoryVM = new CategoryVM();
            bool isCategoryIdExists = HttpContext.Session.TryGetValue("CategoryId", out byte[] _);
            if (!isCategoryIdExists)
            {
                categoryVM.Categories = _repository.Categories.GetAll(filter: x => x.ParentId == 0, orderBy: x => x.OrderBy(y => y.CategoryName)).ToList();
            }
            else
            {
                int id = (int)HttpContext.Session.GetInt32("CategoryId");
                categoryVM.Categories = _repository.Categories.GetAll(filter: x => x.ParentId == id, orderBy: x => x.OrderBy(y => y.CategoryName)).ToList();
                categoryVM.Category = _repository.Categories.GetById(id);
            }
            ViewData["Categories"] = categoryVM;
        }
        private int ProccessPageSize(int PageSize)
        {
            bool isPageSizeInSession = HttpContext.Session.TryGetValue("PageSize", out _);

            if (!isPageSizeInSession)
            {
                PageSize = _pageSize;
                HttpContext.Session.SetString("PageSize", PageSize.ToString());
            }
            else if (PageSize == 0)
            {
                PageSize = int.Parse(HttpContext.Session.GetString("PageSize"));
            }
            else
            {
                HttpContext.Session.SetString("PageSize", PageSize.ToString());
            }
            return PageSize;
        }
        private void BuildBreadcrumbNodeCategoteryTree(Category category, MvcBreadcrumbNode node)
        {
            if (category.ParentId == 0)
            {
                var parentCategoryBreadcrumbNode = new MvcBreadcrumbNode("Index", "Product", "Books");
                node.Parent = parentCategoryBreadcrumbNode;
            }
            else
            {
                var parentCategory = _repository.Categories.GetById(category.ParentId);
                var parentCategoryBreadcrumbNode = new MvcBreadcrumbNode("Category", "Product", parentCategory.CategoryName);
                parentCategoryBreadcrumbNode.RouteValues = new { categoryId = parentCategory.Id };
                node.Parent = parentCategoryBreadcrumbNode;
                BuildBreadcrumbNodeCategoteryTree(parentCategory, parentCategoryBreadcrumbNode);
            }

            ViewData["BreadcrumbNode"] = node;
        }
        #endregion
    }
}
