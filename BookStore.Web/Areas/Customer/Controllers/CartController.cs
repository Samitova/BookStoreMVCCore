using BookStore.Services.ShopService;
using BookStore.Services.ShopService.SearchService;
using BookStore.ViewModelData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartBreadcrumbs.Attributes;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BookStore.Web.Areas.Customer.Controllers
{
    [Area("customer")]
    public class CartController : Controller
    {
        private readonly ShopService _bookService;

        public CartController(ShopService bookService)
        {
            _bookService = bookService;
        }

        [Breadcrumb(Title = "ViewData.Title")]
        public IActionResult Index()
        {
            ViewData["searchBar"] = new SearchBar() { Action = "Index", Controler = "Shop", SearchText = "" };

            CartVM cartItems;
            if (!HttpContext.Session.TryGetObject("Cart", out cartItems))
                cartItems = new CartVM();

            if (cartItems.Items.Count == 0)
            {
                ViewBag.Message = "Your cart is empty";
                return View();
            }
            return View(cartItems);
        }

        //GET Cart/AddToCart/Id       
        public IActionResult AddToCartPartial(int id)
        {
            CartVM cartItems;
            if (!HttpContext.Session.TryGetObject("Cart", out cartItems))
                cartItems = new CartVM();

            CartItem cartItem = new CartItem();
            var book = _bookService.GetBookById(id);
            var productInCart = cartItems.Items.FirstOrDefault(x => x.BookId == id);

            if (productInCart == null)
            {
                cartItems.Items.Add(new CartItem
                {
                    BookId = book.Id,
                    Title = book.Title,
                    AuthorName = book.AuthorFullName,
                    AuthorId = book.AuthorId,
                    AvaliableQuantaty = book.AvaliableQuantaty,
                    Quantity = 1,
                    Price = book.Price,
                    Image = book.PhotoPath
                });
            }
            else
            {
                productInCart.Quantity++;
            }
            cartItems.TotalAmount += 1;
            cartItems.TotalPrice += book.Price;

            HttpContext.Session.SetObject("Cart", cartItems);

            return PartialView("_CartPartial", cartItems);
        }

        // GET: Cart/IncrementProduct       
        public JsonResult IncrementProduct(int bookId)
        {
            CartVM cartItems;
            if (!HttpContext.Session.TryGetObject("Cart", out cartItems))
                cartItems = new CartVM();


            CartItem cartItem = cartItems.Items.FirstOrDefault(x => x.BookId == bookId);

            cartItem.Quantity++;

            HttpContext.Session.SetObject("Cart", cartItems);

            var result = new { qty = cartItem.Quantity, price = cartItem.Price, title = cartItem.Title };

            return Json(result);
        }

        // GET: Cart/DecrementProduct       
        public JsonResult DecrementProduct(int bookId)
        {
            CartVM cartItems;
            if (!HttpContext.Session.TryGetObject("Cart", out cartItems))
                cartItems = new CartVM();

            CartItem cartItem = cartItems.Items.FirstOrDefault(x => x.BookId == bookId);

            if (cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
            }
            else
            {
                cartItem.Quantity = 0;
                cartItems.Items.Remove(cartItem);
            }

            HttpContext.Session.SetObject("Cart", cartItems);
            var result = new { qty = cartItem.Quantity, price = cartItem.Price, title = cartItem.Title };

            return Json(result);
        }

        // GET: Cart/RemoveProduct       
        public void RemoveProduct(int bookId)
        {
            CartVM cartItems;
            if (!HttpContext.Session.TryGetObject("Cart", out cartItems))
                cartItems = new CartVM();

            CartItem cartItem = cartItems.Items.FirstOrDefault(x => x.BookId == bookId);
            cartItems.Items.Remove(cartItem);
            HttpContext.Session.SetObject("Cart", cartItems);
        }
    }
}
