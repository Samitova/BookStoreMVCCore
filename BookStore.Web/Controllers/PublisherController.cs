using AutoMapper;
using BookStore.DataAccess.Contracts;
using BookStore.DataAccess.Models;
using BookStore.Services.Contracts;
using BookStore.Services.Managers;
using BookStore.Services.ShopService.SearchService;
using BookStore.ViewModelData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SmartBreadcrumbs.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    [Breadcrumb("Publishers")]    
    public class PublisherController : Controller
    {
        private readonly IShopManager _shopManager;

        public PublisherController(IShopManager shopManager)
        {
            _shopManager = shopManager;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewData["SearchBar"] = new SearchBar() { Action = "Index", Controler = "Publisher", SearchText = "" };
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            IEnumerable<PublisherViewModel> publishersList = await _shopManager.PublisherManager.GetAllPublishersAsync();
            return View(publishersList);
        }

        [HttpGet]
        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> CreateUpdatePublisher(int? id)
        {
            PublisherViewModel publisher = new PublisherViewModel();
            if (id == null || id == 0)
            {
                return View(publisher);
            }
            else
            {
                publisher = await _shopManager.PublisherManager.GetPublisherByIdAsync(id);
                if (publisher == null)
                    return NotFound();
                else
                {
                    return View(publisher);
                }
            }
        }

        [HttpPost]
        public IActionResult CreateUpdatePublisher(PublisherViewModel publisherVM)
        {
            if (ModelState.IsValid)
            {
                if (publisherVM.Id == 0)
                {
                    _shopManager.PublisherManager.AddPublisher(publisherVM);
                    TempData["success"] = "Publisher was created successfuly";
                }
                else
                {
                    _shopManager.PublisherManager.UpdatePublisher(publisherVM);
                    TempData["success"] = "Publisher was updated successfuly";
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Breadcrumb(Title = "ViewData.Title")]
        public async Task<IActionResult> DeletePublisher(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var publisher = await _shopManager.PublisherManager.GetPublisherByIdAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(publisher);
        }

        [HttpPost, ActionName("DeletePublisher")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePublisherPost(int id)
        {
            PublisherViewModel publisher = await _shopManager.PublisherManager.GetPublisherByIdAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }

            _shopManager.PublisherManager.DeletePublisher(id);
            TempData["success"] = $"Publisher \"{publisher.PublisherName}\" was deleted successfuly";
            return RedirectToAction("Index");
        }
    }
}
