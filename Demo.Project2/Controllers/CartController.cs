using Demo.Project2.Context;
using Demo.Project2.Dtos;
using Demo.Project2.Helper;
using Demo.Project2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        #region Thanh toán giỏ hàng
        [HttpPost]
        [Route("checkout")]
        public async Task<IActionResult> Checkout(Order order)
        {
            var userId = User.FindFirstValue(ClaimTypes.Sid);
            var userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userId == null || userRole != "Customer")
            {
                return RedirectToAction("login", "auth");
            }
            var user = await _context.Users!.FirstOrDefaultAsync(a => a.Id.Equals(Guid.Parse(userId)));
            var createdOn = DateTime.Now;
            var newOrder = new Order
            {
                UserId = Guid.Parse(userId),
                Code = $"DH{createdOn:yyyyMMddHHmmss}",
                Name = $"Đơn hàng online",
                FullName = order.FullName,
                PhoneNumber = order.PhoneNumber,
                Address = order.Address,
                Description = order.Description,
                CreatedOn = createdOn,
                OrderStatus = OrderStatus.Processing
            };
            _context.Add(newOrder);
            var cart = SessionHelper.Get<List<Item>>(HttpContext.Session, "cart");
            foreach (var item in cart!)
            {
                var newOrderDetails = new OrderDetails
                {
                    OrderId = newOrder.Id,
                    ProductId = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity
                };
                _context.Add(newOrderDetails);
            }
            await _context.SaveChangesAsync();
            HttpContext.Session.Remove("cart");
            return RedirectToAction("checkoutResult", "cart");
        }
        #endregion Thanh toán giỏ hàng

        #region Kết quả thanh toán
        [Route("checkoutResult")]
        public IActionResult CheckoutResult()
        {
            return View();
        }
        #endregion Kết quả thanh toán
    }
}
