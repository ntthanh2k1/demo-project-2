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

        
    }
}
