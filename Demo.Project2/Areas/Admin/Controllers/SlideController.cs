using Demo.Project2.Context;
using Demo.Project2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Project2.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminSchemes")]
    [Area("admin")]
    [Route("admin/slide")]
    public class SlideController : Controller
    {
        private readonly DemoProject2DbContext _context;

        public SlideController(DemoProject2DbContext context)
        {
            _context = context;
        }

        #region Trang quản lý slide
        [HttpGet]
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var slides = await _context.Slides!.ToListAsync();
            return View(slides);
        }
        #endregion Trang quản lý slide


    }
}
