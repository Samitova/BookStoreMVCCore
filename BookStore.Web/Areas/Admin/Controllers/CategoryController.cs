using BookStore.Data.Models.ModelsDTO;
using BookStore.Data.Models.ViewModels;
using BookStore.Services.DataBaseService.Interfaces;
using BookStore.Services.ShopService.SearchService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        public CategoryController(IRepositoryWrapper repositoryWrapper)
        {
            _repository = repositoryWrapper;           
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewData["SearchBar"] = new SearchBar() { Action = "Index", Controler = "Category", SearchText = "" };            
        }

        public IActionResult Index()
        {           
            CategoryVM categoryVM = GetTreeVeiwCategories();            
            return View(categoryVM);
        }

        public CategoryVM GetTreeVeiwCategories()
        {
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.Categories = _repository.Categories.GetAll(filter: x => x.ParentId == 0, orderBy: x => x.OrderBy(y => y.CategoryName));

            foreach (var category in categoryVM.Categories)
            {
                BuildSubCategory(category);
            }
            return categoryVM;
        }

        private void BuildSubCategory(Category rootCategory)
        {            
            List<Category> categories = _repository.Categories.GetAll(filter: x => x.ParentId == rootCategory.Id, orderBy: x => x.OrderBy(y => y.CategoryName)).ToList();

            if (categories.Count > 0)
            {
                foreach (var subCategory in categories)
                {
                    BuildSubCategory(subCategory);
                    rootCategory.SubCategory.Add(subCategory);
                }
            }
        }

        [HttpGet]
        public IActionResult CreateUpdateCategory(int? id)
        {           
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.Categories = _repository.Categories.GetAll(orderBy: x => x.OrderBy(y => y.CategoryName)).ToList();

            ViewBag.categories = categoryVM.Categories.Select(i => new SelectListItem()
            {
                Text = i.CategoryName,
                Value = i.Id.ToString()
            });

            if (id==null || id == 0)
            {
                return View(categoryVM);
            }
            else
            {               
                categoryVM.Category = _repository.Categories.GetById(id);
                if(categoryVM.Category == null)
                    return NotFound();
                else
                    return View(categoryVM);    
            }
        }

        [HttpPost]
        public IActionResult CreateUpdateCategory(CategoryVM categoryVM)
        {           
            if (ModelState.IsValid)
            {
                if (categoryVM.Category.Id == 0)
                {
                    _repository.Categories.Add(categoryVM.Category);                   
                    TempData["success"] = "Category was created successfuly";
                }
                else
                {
                    _repository.Categories.Update(categoryVM.Category);
                    TempData["success"] = "Category was updated successfuly";
                }   
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteCategory(int? id)
        {
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
