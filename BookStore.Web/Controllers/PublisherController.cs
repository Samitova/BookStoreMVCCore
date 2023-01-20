using AutoMapper;
using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.Models;
using BookStore.Services.Contracts;
using BookStore.Services.Managers;
using BookStore.Services.ShopService.SearchService;
using BookStore.ViewModelData;
using BookStore.Web.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    [Breadcrumb("Publishers")]    
    public class PublisherController : Controller
    {
        private readonly IShopManager _shopManager;
        private readonly IDataProtector _dataProtector;

        public PublisherController(IShopManager shopManager, IDataProtectionProvider dataProtectionProvider,
                              DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _shopManager = shopManager;
            _dataProtector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.BookIdRouteValue);
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewData["SearchBar"] = new SearchBar() { Action = "Index", Controler = "Publisher", SearchText = "" };
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<PublisherViewModel> publishersList = (await _shopManager.PublisherManager.GetAllPublishersAsync()).Select(e =>
            {
                e.EncryptedId = _dataProtector.Protect(e.Id.ToString());               
                return e;
            }); ;
            return View(publishersList);
        }

        [HttpGet]
        [Breadcrumb(Title = "ViewData.Title")]
        public IActionResult CreatePublisher()
        {
            PublisherViewModel publisher = new PublisherViewModel();
            return View(publisher);           
        }

        [HttpPost]
        public IActionResult CreatePublisher(PublisherViewModel publisher)
        {
            if (ModelState.IsValid)
            {                           
                _shopManager.PublisherManager.AddPublisher(publisher);
                TempData["success"] = "Publisher was created successfuly";
                return RedirectToAction("Index");
            }
            return View(publisher);
        }

        [HttpGet]
        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> EditPublisher(string id)
        {
            int decryptedId = Convert.ToInt32(_dataProtector.Unprotect(id));
            PublisherViewModel publisher = await _shopManager.PublisherManager.GetPublisherByIdAsync(decryptedId);
            if (publisher == null)
                return NotFound();
            else
            {
                return View(publisher);
            }
        }

        [HttpPost]
        public IActionResult EditPublisher(PublisherViewModel publisher)
        {
            if (ModelState.IsValid)
            {
                publisher.Id = _dataProtector.Unprotect(publisher.Id);               
                _shopManager.PublisherManager.UpdatePublisher(publisher);
                TempData["success"] = "Publisher was updated successfuly";
                return RedirectToAction("Index");
            }
            return View(publisher);
        }

        [HttpGet]
        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> DeletePublisher(string id)
        {
            int decryptedId = Convert.ToInt32(_dataProtector.Unprotect(id));
            var publisher = await _shopManager.PublisherManager.GetPublisherByIdAsync(decryptedId);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(publisher);
        }

        [HttpPost, ActionName("DeletePublisher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePublisherPost(string id)
        {
            int decryptedId = Convert.ToInt32(_dataProtector.Unprotect(id));
            PublisherViewModel publisher = await _shopManager.PublisherManager.GetPublisherByIdAsync(decryptedId);
            if (publisher == null)
            {
                return NotFound();
            }

            _shopManager.PublisherManager.DeletePublisher(decryptedId);
            TempData["success"] = $"Publisher \"{publisher.PublisherName}\" was deleted successfuly";
            return RedirectToAction("Index");
        }
    }
}
