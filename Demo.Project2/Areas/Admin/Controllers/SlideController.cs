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
            var imageName = "";
            if (image != null)
            {
                imageName = $"{DateTime.Now:ddMMyyyyHHmmss}_{image.FileName}";
                var path = Path.Combine(_webHostEnvironment.WebRootPath, @"admin\images\slides", imageName);
                using var stream = new FileStream(path, FileMode.Create);
                await image.CopyToAsync(stream);
            }
            var newSlide = new Slide
            {
                Code = slide.Code,
                Name = slide.Name,
                Image = imageName,
                Description = slide.Description,
                IsActive = slide.IsActive
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
            var slide = await _context.Slides!.FirstOrDefaultAsync(a => a.Id.Equals(id));
            return View("details", slide);
        }
        #endregion Xem chi tiết slide

        #region Cập nhật slide
        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var slide = await _context.Slides!.FindAsync(id);
            return View("edit", slide);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id, Slide slide, IFormFile image)
        {
            var currentSlide = await _context.Slides!.FindAsync(id);
            var imageName = "";
            if (image != null)
            {
                imageName = $"{DateTime.Now:ddMMyyyyHHmmss}_{image.FileName}";
                var path = Path.Combine(_webHostEnvironment.WebRootPath, @"admin\images\slides", currentSlide!.Image!);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                path = Path.Combine(_webHostEnvironment.WebRootPath, @"admin\images\slides", imageName);
                using var stream = new FileStream(path, FileMode.Create);
                await image.CopyToAsync(stream);
            }
            currentSlide!.Code = slide.Code;
            currentSlide.Name = slide.Name;
            currentSlide.Image = imageName;
            currentSlide.Description = slide.Description;
            currentSlide.IsActive = slide.IsActive;
            _context.Update(currentSlide);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "slide", new { area = "admin" });
        }
        #endregion Cập nhật slide

        #region Xóa slide
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var slide = await _context.Slides!.FindAsync(id);
            if (slide!.Image != null)
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, @"admin\images\slides", slide.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            _context.Slides.Remove(slide!);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "slide", new { area = "admin" });
        }
        #endregion Xóa slide
    }
}