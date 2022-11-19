using AutoMapper;
using BookStore.Data.Models.ViewModels;
using BookStore.Services.DataBaseService.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    public class ShopController : Controller
    {
        private readonly IRepositoryWrapper _repositoryWrapper;
        private IMapper _mapper;

        public ShopController( IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repositoryWrapper = repositoryWrapper;  
            _mapper = mapper;   
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var booksDTO = await _repositoryWrapper.Books.GetAllAsync(); 
               
                var result = _mapper.Map<IEnumerable<BookVM>>(booksDTO);
                return View(result);
            }
            catch (Exception ex)
            {                
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
