using AutoMapper;
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

namespace BookStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        private SortModel _sortModel = new SortModel();
        private readonly ShopService _bookService;
        private int _pageSize = 4;
        public ProductController(IRepositoryWrapper repositoryWrapper, ShopService bookService)
        {
            _bookService = bookService;
            _repository = repositoryWrapper;
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
            
            var categories = _repository.Categories.GetAll().ToList();
            ViewData["Categories"] = categories;

            books = _bookService.GetAllBooksFromDb("");
            HttpContext.Session.SetString("Books", JsonConvert.SerializeObject(books));

            navigationService.SetNavigationService("Paging", books, 1, _pageSize);

            ViewData["NavigationService"] = navigationService;
            return View(navigationService.PagedBooks);
        }

        private void SetSortModel()
        {
            Dictionary<string, string> sortedProperties = new Dictionary<string, string>() { { "title", "title" }, { "price", "price_desc" }, { "author", "author" }, { "rating", "rating_desc" } };
            _sortModel.InitSortModel(sortedProperties);
            _sortModel.ApplySort("Sort", "");
            ViewData["SortModel"] = _sortModel;
        }

    }
}
