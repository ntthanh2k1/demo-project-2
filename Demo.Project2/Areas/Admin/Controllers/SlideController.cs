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
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SlideController(DemoProject2DbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

        #region Tạo slide
        [HttpGet]
        [Route("create")]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(Slide slide, IFormFile image)
        {
            var filePath = Path
                .Combine(_webHostEnvironment.WebRootPath, @"admin\images\slides", image.FileName);
            var stream = System.IO.File.Create(filePath);
            await image.CopyToAsync(stream);
            var newSlide = new Slide
            {
                Code = slide.Code,
                Name = slide.Name,
                Image = image.FileName,
                Description = slide.Description,
                Status = slide.Status
            };
            _context.Add(newSlide);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "slide", new { area = "admin" });
        }
        #endregion Tạo slide

        #region Xem chi tiết slide
        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            return View("details");
        }
        #endregion Xem chi tiết slide

        #region Cập nhật slide
        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            return View("edit");
        }

        [HttpPost]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id, Slide slide)
        {
            return RedirectToAction("index", "slide", new { area = "admin" });
        }
        #endregion Cập nhật slide

        #region Xóa slide
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var slide = await _context.Slides!.FindAsync(id);
            _context.Slides.Remove(slide!);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "slide", new { area = "admin" });
        }
        #endregion Xóa slide
    }
}