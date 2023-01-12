using BookStore.Services.Contracts;
using BookStore.Services.Managers;
using BookStore.Services.ShopService.SearchService;
using BookStore.ViewModelData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Web.Controllers
{    
    public class CartController : Controller
    {
        private readonly IBookManager _bookManager;

        public CartController(BookManager bookManager)
        {
            _bookManager = bookManager;
        }

        [Breadcrumb(Title = "ViewData.Title")]
        public IActionResult Index()
        {
            ViewData["searchBar"] = new SearchBar() { Action = "Index", Controler = "Book", SearchText = "" };

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
        public async Task<IActionResult> AddToCartPartial(int id)
        {
            CartVM cartItems;
            if (!HttpContext.Session.TryGetObject("Cart", out cartItems))
                cartItems = new CartVM();

            CartItem cartItem = new CartItem();
            var book = await _bookManager.GetBookByIdAsync(id);
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
