using AutoMapper;
using BookStore.Data.Models.ModelsDTO;
using BookStore.Data.Models.ViewModels;
using BookStore.Services.DataBaseService.Interfaces;
using BookStore.Services.ShopService;
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

        public IActionResult Index(string SortExpression="", string SearchText = "")
        {
            SetSortModel(SortExpression);           
            SetSearchPager("Index", "Shop", SearchText);

            try
            {
                var books = _bookService.GetAll(_sortModel.SortedProperty, _sortModel.SortedOrder, SearchText).ToList();                  
                HttpContext.Session.SetString("Books", JsonConvert.SerializeObject(books));
                return View(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        public IActionResult Order(string SortExpression = "")
        {
            SetSortModel(SortExpression);

            ViewData["SearchPager"] = JsonConvert.DeserializeObject<SearchPager>(HttpContext.Session.GetString("SearchPager"));

            var books = JsonConvert.DeserializeObject<List<BookVM>>(HttpContext.Session.GetString("Books"));

            books = _bookService.DoSort(books, _sortModel.SortedProperty, _sortModel.SortedOrder);

            return View("Index", books);           
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

        private void SetSearchPager(string action, string controler, string searchText)
        {            
            SearchPager searchPager = new SearchPager() { Action = action, Controler = controler, SearchText = searchText};
            ViewData["SearchPager"] = searchPager;
            HttpContext.Session.SetString("SearchPager", JsonConvert.SerializeObject(searchPager));
        }
    }
}
