using BookStore.Data.Models.ModelsDTO;
using BookStore.Data.Models.ViewModels;
using BookStore.Services.DataBaseService.Interfaces;
using BookStore.Services.DataBaseService.Repositories;
using BookStore.Services.ShopService;
using BookStore.Services.ShopService.PaginationService;
using BookStore.Services.ShopService.SearchService;
using BookStore.Services.ShopService.SortingService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static System.Reflection.Metadata.BlobBuilder;


namespace BookStore.Web.Areas.Customer.Controllers
{
    [Area("customer")]
    public class ShopController : Controller
    {
        private readonly ShopService _bookService;
        private int _pageSize = 2;
        private SortModel _sortModel = new SortModel();

        public ShopController(ShopService bookService)
        {
            _bookService = bookService;            
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewData["SearchBar"] = new SearchBar() { Action = "Search", Controler = "Shop", SearchText = "" };
            SetSortModel();
        }

        public IActionResult Index()
        {          
            List<BookVM> books = new List<BookVM>();
            NavigationService navigationService = new NavigationService();            
           
            books = _bookService.GetAllBooksFromDb("");           
            HttpContext.Session.SetString("Books", JsonConvert.SerializeObject(books));
           
            navigationService.SetNavigationService("Paging", books, 1, _pageSize);

            ViewData["NavigationService"] = navigationService;
            return View(navigationService.PagedBooks);
        }
                 
        public IActionResult Search(string SearchText = "")
        {   
            List<BookVM>  books = _bookService.GetAllBooksFromDb(SearchText);
            HttpContext.Session.SetString("Books", JsonConvert.SerializeObject(books));

            NavigationService navigationService = new NavigationService();
            navigationService.SetNavigationService("Paging", books, 1, _pageSize);

            ViewData["NavigationService"] = navigationService;
            return View("Index", navigationService.PagedBooks);
        }

        public IActionResult Sort(string sortExpression = "")
        {
            NavigationService navigationService = new NavigationService();          

            _sortModel.ApplySort("Sort", sortExpression);
            ViewData["SortModel"] = _sortModel;

            List<BookVM> books = JsonConvert.DeserializeObject<PaginatedList<BookVM>>(HttpContext.Session.GetString("Books"));
            books = _bookService.DoSort(books, _sortModel.SortedProperty, _sortModel.SortedOrder);
            HttpContext.Session.SetString("Books", JsonConvert.SerializeObject(books));

            navigationService.SetNavigationService("Paging", books, 1, _pageSize);
            ViewData["NavigationService"] = navigationService;
            return View("Index", navigationService.PagedBooks);
        }


        public IActionResult Paging(int PageSize, int CurrentPage=1)
        {
            NavigationService navigationService = new NavigationService();
            if(PageSize!=_pageSize)
                _pageSize = PageSize;
            List<BookVM> books = JsonConvert.DeserializeObject<PaginatedList<BookVM>>(HttpContext.Session.GetString("Books"));
            navigationService.SetNavigationService("Paging", books, CurrentPage, _pageSize);
            ViewData["NavigationService"] = navigationService;
            return View("Index", navigationService.PagedBooks);   
        }

        public IActionResult AuthorBooksPaging(int PageSize, int CurrentPage=1)
        {
            if (PageSize != _pageSize)
                _pageSize = PageSize;
            AuthorVM author = JsonConvert.DeserializeObject<AuthorVM>(HttpContext.Session.GetString("Author"));
            author.Books = JsonConvert.DeserializeObject<PaginatedList<BookVM>>(HttpContext.Session.GetString("Books"));

            NavigationService navigationService = new NavigationService();
            navigationService.SetNavigationService("AuthorBooksPaging", author.Books, CurrentPage, _pageSize);

            ViewData["NavigationService"] = navigationService;
            return View("AuthorDetails", author);
        }

        public IActionResult AuthorDetails(int id, int PageSize, string SortExpression = "")
        {
            NavigationService navigationService = new NavigationService();
            AuthorVM author = new AuthorVM();

            if (PageSize != _pageSize && PageSize != 0)
                _pageSize = PageSize;

            _sortModel.ApplySort("AuthorDetails", SortExpression);
            ViewData["SortModel"] = _sortModel;

            author = _bookService.GetAuthor(id, _sortModel);
            HttpContext.Session.SetString("Author", JsonConvert.SerializeObject(author));
            author.Books = _bookService.DoSort(author.Books, _sortModel.SortedProperty, _sortModel.SortedOrder);
            HttpContext.Session.SetString("Books", JsonConvert.SerializeObject(author.Books));

            navigationService.SetNavigationService("AuthorBooksPaging", author.Books, 1, _pageSize);
            ViewData["NavigationService"] = navigationService;
            return View(author);
        }
        public IActionResult BookDetails(int id)
        {
            BookVM book = _bookService.GetBookById(id);
            return View(book);
        }
        public IActionResult AddBookComment(BookCommentDTO bookComment)
        {
            if (string.IsNullOrEmpty(bookComment.PublisherName))
                bookComment.PublisherName = "Anonimus";
            _bookService.AddBookComment(bookComment);
            BookVM book = _bookService.GetBookById(bookComment.BookId);
            return View("BookDetails", book);
        }

        private void SetSortModel()
        {
            Dictionary<string, string> sortedProperties = new Dictionary<string, string>() { { "title", "title" }, { "price", "price_desc" }, { "author", "author" }, { "rating", "rating_desc" }, { "bestsellers", "bestsellers_desc" }, { "novelties", "novelties_desc" } };
            _sortModel.InitSortModel(sortedProperties);
            _sortModel.ApplySort("Sort", "");
            ViewData["SortModel"] = _sortModel;
        }
    }
}
