using Demo.Project2.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Project2.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminSchemes")]
    [Area("admin")]
    [Route("admin/category")]
    public class CategoryController : Controller
    {
        private readonly DemoProject2DbContext _context;

        public CategoryController(DemoProject2DbContext context)
        {
            _context = context;
        }

        #region Trang phân loại
        [HttpGet]
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            //ViewBag.categories = await _context.Categories
            //    .Where(c => c.ParentCategory == null)
            //    .ToListAsync();
            return View(await _context.Categories.ToListAsync());
        }
        #endregion Trang phân loại
    }
}
