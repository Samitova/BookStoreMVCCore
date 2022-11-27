using AutoMapper;
using BookStore.Data.Models.ModelsDTO;
using BookStore.Data.Models.ViewModels;
using BookStore.Services.DataBaseService.Interfaces;
using BookStore.Services.ShopService;
using BookStore.Services.ShopService.PaginationService;
using BookStore.Services.ShopService.SotrOrderingService;
using BookStore.Web.Models;
using BookStore.Web.Views.Shared.Components.SearchBar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using static System.Reflection.Metadata.BlobBuilder;
using SortOrder = BookStore.Services.ShopService.SotrOrderingService.SortOrder;

namespace BookStore.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;        
        private readonly BookService _bookService;
        private readonly SortModel _sortModel;        

        public ShopController( IRepositoryWrapper repositoryWrapper, BookService bookService, SortModel sortModel)
        {
            _repositoryWrapper = repositoryWrapper;              
            _bookService = bookService; 
            _sortModel = sortModel;
            InitSortModel();
        }      
            

        public IActionResult Index(string SortExpression = "", string SearchText = "", int CurrentPage = 1, int PageSize = 2)
        {
            List<BookVM> books = new List<BookVM>();
            PaginatedList<BookVM> pagedBooks = new PaginatedList<BookVM>();
            string oldSearchText = "";

            SetSortModel(SortExpression);
            SetSearchBar("Index", "Shop", SearchText);   

            bool isBooksInSession = HttpContext.Session.TryGetValue("Books", out _ );     

            if (isBooksInSession) 
            {
                oldSearchText = HttpContext.Session.GetString("SearchText");
            }

            if (!isBooksInSession || (isBooksInSession && oldSearchText != SearchText))
            {
                books = _bookService.GetAllFromDb(SearchText);
                books = _bookService.DoSort(books, _sortModel.SortedProperty, _sortModel.SortedOrder);
                HttpContext.Session.SetString("Books", JsonConvert.SerializeObject(books));
                HttpContext.Session.SetString("SearchText", SearchText);               
            }
            else 
            {
                books = JsonConvert.DeserializeObject<PaginatedList<BookVM>>(HttpContext.Session.GetString("Books"));
                books = _bookService.DoSort(books, _sortModel.SortedProperty, _sortModel.SortedOrder);
            }

            pagedBooks = new PaginatedList<BookVM>(books, CurrentPage, PageSize);
            ViewBag.Pagination = new PaginationModel(pagedBooks.TotalRecord, CurrentPage, SortExpression, SearchText, PageSize);
            return View(pagedBooks);
        }     
        private void InitSortModel()
        {
            _sortModel.AddColumn("title", "title");
            _sortModel.AddColumn("price", "price_desc");
            _sortModel.AddColumn("authorfullname", "authorfullname");
            _sortModel.AddColumn("rating", "rating_desc");
            _sortModel.AddColumn("bestsellers", "bestsellers_desc");
            _sortModel.AddColumn("novelties", "novelties_desc");
        }

        private void SetSortModel(string SortExpression)
        {
            _sortModel.ApplySort(SortExpression);
            ViewData["SortModel"] = _sortModel;
        }

        private void SetSearchBar(string action, string controler, string searchText)
        {            
            SearchPager searchBar = new SearchPager() { Action = action, Controler = controler, SearchText = searchText};
            ViewData["SearchBar"] = searchBar;           
        }
    }
}
