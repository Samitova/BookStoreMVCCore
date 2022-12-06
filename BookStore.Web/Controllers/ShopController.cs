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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using static System.Reflection.Metadata.BlobBuilder;


namespace BookStore.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopService _bookService;        
        private int _pageSize = 4;       

        public ShopController(ShopService bookService)
        {
            _bookService = bookService;           
        }

        public IActionResult Index(int PageSize, string SortExpression = "", string SearchText = "", int CurrentPage = 1)
        {
            Dictionary<string, string> sortedProperties = new Dictionary<string, string>() { {"title", "title"}, {"price", "price_desc"},{"authorfullname", "authorfullname"},{"rating", "rating_desc"},{"bestsellers", "bestsellers_desc"},{"novelties", "novelties_desc"}};
            List<BookVM> books = new List<BookVM>();
            NavigationService navigationService = new NavigationService();
            string oldSearchText = "";

            ViewData["searchBar"] = new SearchBar() { Action = "Index", Controler = "Shop", SearchText = SearchText};
            PageSize = ProccessPageSize(PageSize);
            SortModel sortModel = SetSortModel("Index", sortedProperties, SortExpression);    

            bool isBooksInSession = HttpContext.Session.TryGetValue("Books", out _);

            if (isBooksInSession)
            {
                oldSearchText = HttpContext.Session.GetString("SearchText");
            }

            if (!isBooksInSession || (isBooksInSession && oldSearchText != SearchText) || (isBooksInSession && SearchText==""))
            {
                books = _bookService.GetAllBooksFromDb(SearchText);
                books = _bookService.DoSort(books, sortModel.SortedProperty, sortModel.SortedOrder);
                HttpContext.Session.SetString("Books", JsonConvert.SerializeObject(books));
                HttpContext.Session.SetString("SearchText", SearchText);
            }
            else
            {
                books = JsonConvert.DeserializeObject<PaginatedList<BookVM>>(HttpContext.Session.GetString("Books"));
                books = _bookService.DoSort(books, sortModel.SortedProperty, sortModel.SortedOrder);
            }

            navigationService.SetNavigationService("Index", books, CurrentPage, PageSize, SearchText, SortExpression);

            ViewData["NavigationService"] = navigationService;
            return View(navigationService.PagedBooks);
        }

        public IActionResult BookDetails(int id)
        {
            ViewData["searchBar"] = new SearchBar() { Action = "Index", Controler = "Shop", SearchText = "" };
            BookVM book = _bookService.GetBookById(id);
            return View(book);
        }
        public IActionResult AddBookComment(BookCommentDTO bookComment)
        {
            ViewData["searchBar"] = new SearchBar() { Action = "Index", Controler = "Shop", SearchText = "" };
            if (string.IsNullOrEmpty(bookComment.PublisherName))
                bookComment.PublisherName = "Anonimus";            
            _bookService.AddBookComment(bookComment);
            BookVM book = _bookService.GetBookById(bookComment.BookId);
            return View("BookDetails", book);
        }

        public IActionResult AuthorDetails(int id, int PageSize, string SortExpression = "", int CurrentPage = 1)
        {
            Dictionary<string, string> sortedProperties = new Dictionary<string, string>() { { "title", "title" }, { "price", "price_desc" }, { "authorfullname", "authorfullname" }, { "rating", "rating_desc" }, { "bestsellers", "bestsellers_desc" }, { "novelties", "novelties_desc" } };
            NavigationService navigationService = new NavigationService();
            AuthorVM author = new AuthorVM();
            ViewData["searchBar"] = new SearchBar() { Action = "Index", Controler = "Shop", SearchText = "" };
            PageSize = ProccessPageSize(PageSize);
            SortModel sortModel = SetSortModel("AuthorDetails", sortedProperties, SortExpression);

            bool isAuthorInSession = HttpContext.Session.TryGetValue("Author", out _);

            if (!isAuthorInSession)
            {
                author = _bookService.GetAuthor(id, sortModel);
                HttpContext.Session.SetString("Author", JsonConvert.SerializeObject(author));
            }
            else
            {
                author = JsonConvert.DeserializeObject<AuthorVM>(HttpContext.Session.GetString("Author"));
            }
            
            navigationService.SetNavigationService("AuthorDetails", author.Books, CurrentPage, PageSize, "", SortExpression);
            ViewData["NavigationService"] = navigationService;
            return View(author);
        }

        private SortModel SetSortModel(string action, Dictionary<string, string> sortedProperties, string sortExpression = "")
        {            
            SortModel sortModel = new SortModel();
            sortModel.InitSortModel(action, sortedProperties);  
            sortModel.ApplySort(sortExpression);
            ViewData["SortModel"] = sortModel;
            return sortModel;
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
    }
}
