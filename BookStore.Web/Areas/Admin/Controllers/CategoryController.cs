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

        [HttpGet]
        public IActionResult CreateUpdateCategory(int? id)
        {
            ViewData["searchBar"] = new SearchBar() { Action = "Index", Controler = "Shop", SearchText = "" };
            CategoryVM categoryVM = new CategoryVM();
            
            if (id==null || id == 0)
            {
                return View(categoryVM);
            }
            else
            {               
                categoryVM.CategoryDTO = _repository.Categories.GetById(id);
                if(categoryVM.CategoryDTO == null)
                    return NotFound();
                else
                    return View(categoryVM);    
            }
        }

        [HttpPost]
        public IActionResult CreateUpdateCategory(CategoryVM categoryVM)
        {
            ViewData["searchBar"] = new SearchBar() { Action = "Index", Controler = "Shop", SearchText = "" };
            if (ModelState.IsValid)
            {
                if (categoryVM.CategoryDTO.Id == 0)
                {
                    _repository.Categories.Add(categoryVM.CategoryDTO);
                    TempData["success"] = "Category was created successfuly";
                }
                else
                {
                    _repository.Categories.Update(categoryVM.CategoryDTO);
                    TempData["success"] = "Category was updated successfuly";
                }                
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult DeleteCategory(int? id)
        {
            ViewData["searchBar"] = new SearchBar() { Action = "Index", Controler = "Shop", SearchText = "" };
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var category = _repository.Categories.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategoryPost(int? id)
        {
            ViewData["searchBar"] = new SearchBar() { Action = "Index", Controler = "Shop", SearchText = "" };
           
            var category = _repository.Categories.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            _repository.Categories.Delete(category);
            TempData["success"] = "Category was deleted successfuly";
            return RedirectToAction("Index");
        }
    }
}
