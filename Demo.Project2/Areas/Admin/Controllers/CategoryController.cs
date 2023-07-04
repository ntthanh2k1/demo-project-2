using Demo.Project2.Context;
using Demo.Project2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            return View(await _context.Categories
                .Where(a => a.ParentCategory == null)
                .OrderByDescending(b => b.Id)
                .ToListAsync());
        }
        #endregion Trang phân loại

        #region Thêm phân loại cha
        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(Category category)
        {
            var newCategory = new Category
            {
                ParentId = null,
                Name = category.Name,
                Status = category.Status
            };
            _context.Add(newCategory);
            await _context.SaveChangesAsync();
            ViewBag.Success = "Cập nhật thành công.";
            return RedirectToAction("index", "category", new { area = "admin" });
        }
        #endregion Thêm phân loại cha
    }
}
