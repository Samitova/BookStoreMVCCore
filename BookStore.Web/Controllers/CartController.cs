using BookStore.Services.Contracts;
using BookStore.Services.Managers;
using BookStore.Services.ShopService.SearchService;
using BookStore.ViewModelData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{
    [Breadcrumb(Title = "ViewData.Title")]
    [AllowAnonymous]
    public class CartController : Controller
    {
        private readonly IShopManager _shopManager;

        public CartController(IShopManager shopManager)
        {
            _shopManager = shopManager;
        }
      
        public IActionResult Index()
        {
            ViewData["searchBar"] = new SearchBar() { Action = "Index", Controler = "Book", SearchText = "" };

            CartViewModel cartItems;
            if (!HttpContext.Session.TryGetObject("Cart", out cartItems))
                cartItems = new CartViewModel();

            if (cartItems.Items.Count == 0)
            {
                ViewBag.Message = "Your cart is empty";
                return View();
            }
            return View(cartItems);
        }

        //GET Cart/AddToCart/Id
        [HttpGet]        
        public async Task<IActionResult> AddToCartPartial(int id)
        {
            CartViewModel cartItems;
            if (!HttpContext.Session.TryGetObject("Cart", out cartItems))
                cartItems = new CartViewModel();

            CartItem cartItem = new CartItem();
            var book = await _shopManager.BookManager.GetBookByIdAsync(id);
            var productInCart = cartItems.Items.FirstOrDefault(x => x.BookId == id);

            if (productInCart == null)
            {
                cartItems.Items.Add(new CartItem
                {
                    BookId = id,
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
            CartViewModel cartItems;
            if (!HttpContext.Session.TryGetObject("Cart", out cartItems))
                cartItems = new CartViewModel();


            CartItem cartItem = cartItems.Items.FirstOrDefault(x => x.BookId == bookId);

            cartItem.Quantity++;
            cartItems.TotalAmount++;
            cartItems.TotalPrice += cartItem.Price;

            HttpContext.Session.SetObject("Cart", cartItems);

            var result = new { qty = cartItem.Quantity, price = cartItem.Price, title = cartItem.Title };

            return Json(result);
        }

        // GET: Cart/DecrementProduct       
        public JsonResult DecrementProduct(int bookId)
        {
            CartViewModel cartItems;
            if (!HttpContext.Session.TryGetObject("Cart", out cartItems))
                cartItems = new CartViewModel();

            CartItem cartItem = cartItems.Items.FirstOrDefault(x => x.BookId == bookId);

            cartItems.TotalAmount--;
            cartItems.TotalPrice -= cartItem.Price;
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
        public void RemoveProduct(int bookId, int bookQuantity)
        {
            CartViewModel cartItems;
            if (!HttpContext.Session.TryGetObject("Cart", out cartItems))
                cartItems = new CartViewModel();

            CartItem cartItem = cartItems.Items.FirstOrDefault(x => x.BookId == bookId);
            cartItems.TotalAmount-=bookQuantity;
            cartItems.TotalPrice -= cartItem.Price*bookQuantity;
            cartItems.Items.Remove(cartItem);
            HttpContext.Session.SetObject("Cart", cartItems);
        }
    }
}
