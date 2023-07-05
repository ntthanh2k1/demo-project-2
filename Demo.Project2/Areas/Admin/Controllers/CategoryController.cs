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
                .OrderByDescending(a => a.Id)
                .ToListAsync());
        }
        #endregion Trang phân loại

        #region Tạo phân loại
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
            ViewBag.Success = "Tạo thành công.";
            return RedirectToAction("index", "category", new { area = "admin" });
        }
        #endregion Tạo phân loại

        #region Tạo phân loại con
        [HttpGet]
        [Route("createChild")]
        public async Task<IActionResult> CreateChild()
        {
            ViewBag.ParentId = new SelectList(await _context.Categories
                .Where(a => a.ParentCategory == null)
                .ToListAsync(),
                "Id",
                "Name");
            return View();
        }

        [HttpPost]
        [Route("createChild")]
        public async Task<IActionResult> CreateChild(Category category)
        {
            var newCategory = new Category
            {
                ParentId = category.ParentId,
                Name = category.Name,
                Status = category.Status
            };
            _context.Add(newCategory);
            await _context.SaveChangesAsync();
            ViewBag.Success = "Tạo thành công.";
            return RedirectToAction("index", "category", new { area = "admin" });
        }
        #endregion Tạo phân loại con

        #region Cập nhật phân loại
        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            ViewBag.ParentId = new SelectList(await _context.Categories
                .Where(a => a.ParentCategory == null)
                .ToListAsync(),
                "Id",
                "Name");
            return View("edit", category);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            var currentCategory = await _context.Categories.FindAsync(id);
            if (currentCategory.ParentId == null)
            {
                currentCategory.Name = category.Name;
                currentCategory.Status = category.Status;
                _context.Update(currentCategory);
                await _context.SaveChangesAsync();
                ViewBag.Success = "Cập nhật thành công.";
                return RedirectToAction("index", "category", new { area = "admin" });
            }
            currentCategory.ParentId = category.ParentId;
            currentCategory.Name = category.Name;
            currentCategory.Status = category.Status;
            _context.Update(currentCategory);
            await _context.SaveChangesAsync();
            ViewBag.Success = "Cập nhật thành công.";
            return RedirectToAction("index", "category", new { area = "admin" });
        }
        #endregion Cập nhật phân loại

        #region Xóa phân loại
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "category", new { area = "admin" });
        }
        #endregion Xóa phân loại
    }
}
