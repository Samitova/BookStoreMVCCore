using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.Models;
using BookStore.Services.Contracts;
using BookStore.Services.ShopService.SearchService;
using BookStore.ViewModelData;
using BookStore.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<AdministrationController> _logger;
        private readonly IDataProtector _dataProtector;
        public CategoryController(IShopManager shopManager, IDataProtectionProvider dataProtectionProvider,
                              DataProtectionPurposeStrings dataProtectionPurposeStrings, ILogger<AdministrationController> logger)
        {
            _shopManager = shopManager;
            _logger = logger;
            _dataProtector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.BookIdRouteValue);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewData["SearchBar"] = new SearchBar() { Action = "Index", Controler = "BrowseCategory", SearchText = "" };
        }

        public IActionResult Index()
        {
            CategoryViewModel categoryVM = GetTreeVeiwCategories();
            categoryVM.Categories = categoryVM.Categories.Select(e =>
            {
                e.EncryptedId = _dataProtector.Protect(e.Id.ToString());
                return e;
            });
            return View(categoryVM);
        }
        
        public CategoryViewModel GetTreeVeiwCategories()
        {
            CategoryViewModel categoryVM = new CategoryViewModel();
            categoryVM.Categories = _shopManager.CategoryManager.GetSubCategories(0);
            categoryVM.Categories = categoryVM.Categories.Select(e =>
            {
                e.EncryptedId = _dataProtector.Protect(e.Id.ToString());
                return e;
            });

            foreach (var category in categoryVM.Categories)
            {
                BuildSubCategory(category);
            }
            return categoryVM;
        }
        
        private void BuildSubCategory(Category rootCategory)
        {
            IEnumerable<Category> categories = _shopManager.CategoryManager.GetSubCategories(rootCategory.Id).Select(e =>
            {
                e.EncryptedId = _dataProtector.Protect(e.Id.ToString());
                return e;
            });       

            if (categories.ToList().Count > 0)
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
            CategoryViewModel categoryVM = new CategoryViewModel();
            categoryVM.Categories = await _shopManager.CategoryManager.GetAllCategoriesAsync();               

            ViewBag.categories = categoryVM.Categories.Select(i => new SelectListItem()
            {
                Text = i.CategoryName,
                Value = i.Id.ToString()
            });           
            return View(categoryVM);          
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryViewModel categoryVM)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _shopManager.CategoryManager.AddCategory(categoryVM.Category);
                    TempData["success"] = $"{categoryVM.Category} was created successfuly";
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
        public async Task<IActionResult> EditCategory(string id)
        {
            int decryptedId = Convert.ToInt32(_dataProtector.Unprotect(id));
            CategoryViewModel categoryVM = new CategoryViewModel();
            categoryVM.Categories = await _shopManager.CategoryManager.GetAllCategoriesAsync();

            var excludedCategoryId = new List<int> { decryptedId };
            var filteredCategories = categoryVM.Categories.Where(i => !excludedCategoryId.Contains(i.Id));

            ViewBag.categories = filteredCategories.Select(i =>            
                new SelectListItem()
                {
                    Text = i.CategoryName,
                    Value = i.Id.ToString()
                });
           
            categoryVM.Category = _shopManager.CategoryManager.GetCategoryById(decryptedId);
            if (categoryVM.Category == null)
                return NotFound();
            else
                return View(categoryVM);            
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryViewModel categoryVM)
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
        public IActionResult DeleteCategory(string id)
        {
            int decryptedId = Convert.ToInt32(_dataProtector.Unprotect(id));
            var category = _shopManager.CategoryManager.GetCategoryById(decryptedId);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCategoryPost(string id)
        {
            int decryptedId = Convert.ToInt32(_dataProtector.Unprotect(id));
            var category = _shopManager.CategoryManager.GetCategoryById(decryptedId);
            if (category == null)
            {
                return NotFound();
            }        
            try
            {
                _shopManager.CategoryManager.DeleteCategory(decryptedId);
                TempData["success"] = $"BrowseCategory \"{category.CategoryName}\" was deleted successfuly";
                return RedirectToAction("Index"); ;
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Error deleting category {category.CategoryName}. {ex}");
                ViewBag.ErrorTitle = $"{category.CategoryName} category is in use";
                ViewBag.ErrorMessage = $"{category.CategoryName} category cannot be deleted as there are book with this category. " +
                    $"If you want to delete this category, please remove all books with this category and try again";
                return View("Error");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting category {category.CategoryName}. {ex}");
                return View("Error");
            }
        }
    }
}
