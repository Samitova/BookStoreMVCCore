using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.Models;
using BookStore.Services.Contracts;
using BookStore.Services.ShopService.SearchService;
using BookStore.ViewModelData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    [Breadcrumb("Categories")]
    public class CategoryController : Controller
    {
        private readonly IShopManager _shopManager;
        public CategoryController(IShopManager shopManager)
        {
            _shopManager = shopManager;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewData["SearchBar"] = new SearchBar() { Action = "Index", Controler = "BrowseCategory", SearchText = "" };
        }

        public IActionResult Index()
        {
            CategoryVM categoryVM = GetTreeVeiwCategories();
            return View(categoryVM);
        }

        public CategoryVM GetTreeVeiwCategories()
        {
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.Categories = _shopManager.CategoryManager.GetSubCategories(0);

            foreach (var category in categoryVM.Categories)
            {
                BuildSubCategory(category);
            }
            return categoryVM;
        }

        private void BuildSubCategory(Category rootCategory)
        {
            List<Category> categories = _shopManager.CategoryManager.GetSubCategories(rootCategory.Id).ToList();
           
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
        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> CreateCategory(int id)
        {
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.Categories = await _shopManager.CategoryManager.GetAllCategoriesAsync();               

            ViewBag.categories = categoryVM.Categories.Select(i => new SelectListItem()
            {
                Text = i.CategoryName,
                Value = i.Id.ToString()
            });           
            return View(categoryVM);          
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _shopManager.CategoryManager.AddCategory(categoryVM.Category);
                    TempData["success"] = "BrowseCategory was created successfuly";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(categoryVM);
                }               
            }
          
            return View(categoryVM);
        }

        [HttpGet]
        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            CategoryVM categoryVM = new CategoryVM();
            categoryVM.Categories = await _shopManager.CategoryManager.GetAllCategoriesAsync();

            var excludedCategoryId = new List<int> { id };
            var filteredCategories = categoryVM.Categories.Where(i => !excludedCategoryId.Contains(i.Id));

            ViewBag.categories = filteredCategories.Select(i =>            
                new SelectListItem()
                {
                    Text = i.CategoryName,
                    Value = i.Id.ToString()
                });
           
            categoryVM.Category = _shopManager.CategoryManager.GetCategoryById(id);
            if (categoryVM.Category == null)
                return NotFound();
            else
                return View(categoryVM);            
        }

        [HttpPost]
        public IActionResult UpdateCategory(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _shopManager.CategoryManager.UpdateCategory(categoryVM.Category);
                    TempData["success"] = "BrowseCategory was updated successfuly";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(categoryVM);
                }                               
            }
            return View(categoryVM);
        }

        [HttpGet]
        [Breadcrumb(Title = "ViewData.Title")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _shopManager.CategoryManager.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategoryPost(int id)
        {
            var category = _shopManager.CategoryManager.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            try
            {
                _shopManager.CategoryManager.DeleteCategory(id);
                TempData["success"] = $"BrowseCategory \"{category.CategoryName}\" was deleted successfuly";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(category);
            }           
        }
    }
}
