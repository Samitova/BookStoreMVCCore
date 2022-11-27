using BookStore.Data.Models.ViewModels;
using BookStore.Services.DataBaseService.Interfaces;
using BookStore.Services.ShopService;
using BookStore.Services.ShopService.PaginationService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace BookStore.Web.Controllers
{
    public class ShopController : Controller
    {              
        private readonly BookService _bookService;
        private readonly NavigationService _navigationService;

        public ShopController( BookService bookService, NavigationService navigationService)
        {                        
            _bookService = bookService;
            _navigationService = navigationService;
        }      
            

        public IActionResult Index(string SortExpression = "", string SearchText = "", int CurrentPage = 1, int PageSize = 5)
        {
            List<BookVM> books = new List<BookVM>();           
            string oldSearchText = "";

            _navigationService.SetSearchBar(SearchText);
            _navigationService.SetOrderModel(SortExpression);
 

            bool isBooksInSession = HttpContext.Session.TryGetValue("Books", out _ );     

            if (isBooksInSession) 
            {
                oldSearchText = HttpContext.Session.GetString("SearchText");
            }

            if (!isBooksInSession || (isBooksInSession && oldSearchText != SearchText))
            {
                books = _bookService.GetAllFromDb(SearchText);
                books = _bookService.DoSort(books, _navigationService.SortModel.SortedProperty, 
                                            _navigationService.SortModel.SortedOrder);
                HttpContext.Session.SetString("Books", JsonConvert.SerializeObject(books));
                HttpContext.Session.SetString("SearchText", SearchText);               
            }
            else 
            {
                books = JsonConvert.DeserializeObject<PaginatedList<BookVM>>(HttpContext.Session.GetString("Books"));
                books = _bookService.DoSort(books, _navigationService.SortModel.SortedProperty, 
                                            _navigationService.SortModel.SortedOrder);
            }

            _navigationService.SetNavigationService(books, CurrentPage, PageSize, SearchText, SortExpression);
           
            ViewData["NavigationService"] = _navigationService;
            return View(_navigationService.PagedBooks);
        }  
    }
}
