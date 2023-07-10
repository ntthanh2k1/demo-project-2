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

        #region Trang quản lý phân loại
        [HttpGet]
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.Where(a => a.ParentCategory == null).ToListAsync();
            return View(categories);
        }
        #endregion Trang quản lý phân loại

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
                Id = new Guid(),
                ParentId = null,
                Code = category.Code,
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
                .ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [Route("createChild")]
        public async Task<IActionResult> CreateChild(Category category)
        {
            var newChildCategory = new Category
            {
                Id = new Guid(),
                ParentId = category.ParentId,
                Code = category.Code,
                Name = category.Name,
                Status = category.Status
            };
            _context.Add(newChildCategory);
            await _context.SaveChangesAsync();
            ViewBag.Success = "Tạo thành công.";
            return RedirectToAction("index", "category", new { area = "admin" });
        }
        #endregion Tạo phân loại con

        #region Cập nhật phân loại
        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            return View("edit", category);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id, Category category)
        {
            var currentCategory = await _context.Categories.FindAsync(id);
            currentCategory.Code = category.Code;
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
        public async Task<IActionResult> Delete(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "category", new { area = "admin" });
        }
        #endregion Xóa phân loại
    }
}
