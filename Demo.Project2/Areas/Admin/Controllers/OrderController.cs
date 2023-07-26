using Demo.Project2.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Project2.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminSchemes")]
    [Area("admin")]
    [Route("admin/order")]
    public class OrderController : Controller
    {
        private readonly DemoProject2DbContext _context;

        public OrderController(DemoProject2DbContext context)
        {
            _context = context;
        }

        #region Trang quản lý đơn hàng
        [HttpGet]
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        #endregion Trang quản lý đơn hàng
    }
}
