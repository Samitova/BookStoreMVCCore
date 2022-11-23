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


namespace BookStore.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly BookService _bookService;

        public ShopController( IRepositoryWrapper repositoryWrapper, IMapper mapper, BookService bookService)
        {
            _repositoryWrapper = repositoryWrapper;  
            _mapper = mapper;   
            _bookService = bookService; 
        }      

        public IActionResult Index(string SortExpression="", string SearchText = "")
        {
            SortModel sortModel = SetSortModel(SortExpression);
            SearchPager searchPager = new SearchPager() { Action = "Index", Controler = "Shop", SearchText = SearchText};

            SetIndexViewParametrs(searchPager, sortModel);

            try
            {
                var booksDTO = _bookService.GetAll(SearchText:SearchText).ToList();  
                var result = _mapper.Map<IEnumerable<BookVM>>(booksDTO).ToList();
                HttpContext.Session.SetString("Books", JsonConvert.SerializeObject(result));
                return View(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        public IActionResult Order(string SortExpression = "")
        {    
            var sortModel = JsonConvert.DeserializeObject<SortModel>(HttpContext.Session.GetString("SortModel"));
            sortModel.ApplySort(SortExpression);
            ViewData["SortModel"] = sortModel;
            ViewData["SearchPager"] = JsonConvert.DeserializeObject<SearchPager>(HttpContext.Session.GetString("SearchPager"));

            var books = JsonConvert.DeserializeObject<List<BookVM>>(HttpContext.Session.GetString("Books"));
            books = _bookService.DoSort(books, sortModel.SortedProperty, sortModel.SortedOrder);

            return View("Index", books);           
        }

        private SortModel SetSortModel(string SortExpression)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("title");
            sortModel.AddColumn("price");
            sortModel.AddColumn("authorfullname");
            sortModel.AddColumn("rating");
            sortModel.ApplySort(SortExpression);
            return sortModel;
        }

        private void SetIndexViewParametrs(SearchPager SearchPager, SortModel SortModel)
        {
            ViewData["SortModel"] = SortModel;
            HttpContext.Session.SetString("SortModel", JsonConvert.SerializeObject(SortModel));

            ViewData["SearchPager"] = SearchPager;
            HttpContext.Session.SetString("SearchPager", JsonConvert.SerializeObject(SearchPager));
        }
    }
}
