using BookStore.Data.Models.ViewModels;
using BookStore.Services.ShopService;
using BookStore.Services.ShopService.PaginationService;
using BookStore.Services.ShopService.SearchService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookStore.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopService _bookService;

        public CartController(ShopService bookService)
        {
            _bookService = bookService;
        }
        public IActionResult Index()
        {
            ViewData["searchBar"] = new SearchBar() { Action = "Index", Controler = "Shop", SearchText = "" };

            CartVM cartItems;
            if (!HttpContext.Session.TryGetObject<CartVM>("Cart", out cartItems))
                cartItems = new CartVM();
            
            if (cartItems.Items.Count == 0 )
            {
                ViewBag.Message = "Your cart is empty";
                return View();
            }
            return View(cartItems);
        }              
    }
}
