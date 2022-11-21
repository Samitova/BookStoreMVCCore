using AutoMapper;
using BookStore.Data.Models.ModelsDTO;
using BookStore.Data.Models.ViewModels;
using BookStore.Services.DataBaseService.Interfaces;
using BookStore.Services.ShopService;
using BookStore.Web.Views.Shared.Components.SearchBar;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IActionResult> Index(string SearchText="")
        {
            List <BookVM> result = new List<BookVM>();
            if (!String.IsNullOrWhiteSpace(SearchText))
            {  
                try
                {
                    var booksDTO = await _bookService.GetAllByQuaryAsync(SearchText);
                    result = _mapper.Map<IEnumerable<BookVM>>(booksDTO).ToList();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error");
                }
            }
            else
            {
                try
                {
                    var booksDTO = await _repositoryWrapper.Books.GetAllAsync();
                    result = _mapper.Map<IEnumerable<BookVM>>(booksDTO).ToList();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, "Internal server error");
                }
            }

            SearchPager SearchPager = new SearchPager() { Action="Index", Controler="Shop", SearchText=SearchText};
            ViewBag.SearchPager= SearchPager;
           
            return View(result);
        }      
    }
}
