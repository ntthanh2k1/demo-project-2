using Demo.Project2.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Project2.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminSchemes")]
    [Area("admin")]
    [Route("admin")]
    public class HomeController : Controller
    {
        private readonly DemoProject2DbContext _context;

        public HomeController(DemoProject2DbContext context)
        {
            _context = context;
        }

        #region Trang chủ quản lý
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            ViewBag.CountChildCategories = await _context.Categories!
                .CountAsync(a => a.IsActive && a.ParentCategory != null);
            ViewBag.CountProducts = await _context.Products!.CountAsync(a => a.IsActive);
            return View();
        }
        #endregion Trang chủ quản lý
    }
}