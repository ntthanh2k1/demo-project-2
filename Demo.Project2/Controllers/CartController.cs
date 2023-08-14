using Demo.Project2.Context;
using Demo.Project2.Dtos;
using Demo.Project2.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Project2.Controllers
{
    [Authorize(Roles = "Customer", AuthenticationSchemes = "CustomerSchemes")]
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly DemoProject2DbContext _context;

        public CartController(DemoProject2DbContext context)
        {
            _context = context;
        }

        #region Trang giỏ hàng
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            var cart = SessionHelper.Get<List<Item>>(HttpContext.Session, "cart");
            ViewBag.Cart = cart;
            if (cart == null)
            {
                ViewBag.CountItems = 0;
                ViewBag.TotalPrice = 0;
            }
            else
            {
                ViewBag.countItems = cart.Count;
                ViewBag.TotalPrice = cart.Sum(a => a.Price * a.Quantity);
            }
            return View();
        }
        #endregion Trang giỏ hàng

        #region Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        [Route("addToCart")]
        public async Task<IActionResult> AddToCart(Guid id, int quantity)
        {
            var product = await _context.Products!.FindAsync(id);
            var cart = SessionHelper.Get<List<Item>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cart = new List<Item>
                {
                    new Item
                    {
                        Id = product!.Id,
                        Name = product.Name,
                        Image = product.Image,
                        Price = product.Price,
                        Quantity = quantity
                    }
                };
                SessionHelper.Set(HttpContext.Session, "cart", cart);
            }
            else
            {
                int index = ItemExists(id, cart);
                if (index == -1)
                {
                    cart.Add(new Item
                    {
                        Id = product!.Id,
                        Name = product.Name,
                        Image = product.Image,
                        Price = product.Price,
                        Quantity = quantity
                    });
                }
                else
                {
                    cart![index].Quantity++;
                }
                SessionHelper.Set(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("index", "cart");
        }

        private static int ItemExists(Guid id, List<Item>? cart)
        {
            for (var i = 0; i < cart!.Count; i++)
            {
                if (cart[i].Id == id)
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion Thêm sản phẩm vào giỏ hàng
    }
}
