using AutoMapper;
using BookStore.Data.Models.ViewModels;
using BookStore.Services.DataBaseService.Interfaces;
using BookStore.Services.ShopService.SearchService;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BookStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IMapper _mapper;
        public CategoryController(IRepositoryWrapper repositoryWrapper, IMapper mapper)
        {
            _repository = repositoryWrapper;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            ViewData["searchBar"] = new SearchBar() { Action = "Index", Controler = "Shop", SearchText = "" };
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.Categories = _repository.Categories.GetAll(orderBy: x=>x.OrderBy(y=>y.CategoryName));
            return View(categoryVM);
        }
    }
}
