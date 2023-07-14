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
    [Route("admin/product")]
    public class ProductController : Controller
    {
        private readonly DemoProject2DbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(DemoProject2DbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Trang quản lý sản phẩm
        [HttpGet]
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products!.ToListAsync();
            return View(products);
        }
        #endregion Trang quản lý sản phẩm

        #region Tạo sản phẩm
        [HttpGet]
        [Route("create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.CategoryId = new SelectList(await _context.Categories!
                .Where(a => a.IsActive == true && a.ParentCategory != null)
                .ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create(Product product, IFormFile image)
        {
            var newProduct = new Product
            {
                Id = new Guid(),
                CategoryId = product.CategoryId,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                Details = product.Details,
                Price = product.Price,
                Stock = product.Stock,
                IsFeatured = product.IsFeatured,
                IsActive = product.IsActive
            };
            if (image != null)
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, @"admin\images\products", image.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                await image.CopyToAsync(stream);
                newProduct.Image = image.FileName;
            }
            if (await _context.Products!.AnyAsync(a => a.Id.Equals(newProduct.Id)))
            {
                ViewBag.Error = "Id đã tồn tại, cần nhấn tạo lần nữa.";
                return View("create", newProduct);
            }
            _context.Add(newProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "product", new { area = "admin" });
        }
        #endregion Tạo sản phẩm

        #region Xem chi tiết sản phẩm
        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _context.Products!.FirstOrDefaultAsync(a => a.Id.Equals(id));
            return View("details", product);
        }
        #endregion Xem chi tiết sản phẩm

        #region Cập nhật sản phẩm
        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var product = await _context.Products!.FindAsync(id);
            ViewBag.CategoryId = new SelectList(await _context.Categories!
                .Where(a => a.IsActive == true && a.ParentCategory != null)
                .ToListAsync(), "Id", "Name");
            return View("edit", product);
        }

        [HttpPost]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id, Product product, IFormFile image)
        {
            var currentProduct = await _context.Products!.FindAsync(id);
            currentProduct!.CategoryId = product.CategoryId;
            currentProduct.Code = product.Code;
            currentProduct.Name = product.Name;
            currentProduct.Description = product.Description;
            currentProduct.Details = product.Details;
            currentProduct.Price = product.Price;
            currentProduct.Stock = product.Stock;
            currentProduct.IsFeatured = product.IsFeatured;
            currentProduct.IsActive = product.IsActive;
            if (image != null)
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, @"admin\images\products", currentProduct!.Image!);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                path = Path.Combine(_webHostEnvironment.WebRootPath, @"admin\images\products", image.FileName);
                using var stream = new FileStream(path, FileMode.Create);
                await image.CopyToAsync(stream);
                currentProduct.Image = image.FileName;
            }
            _context.Update(currentProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "product", new { area = "admin" });
        }
        #endregion Cập nhật sản phẩm

        #region Xóa sản phẩm
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _context.Products!.FindAsync(id);
            if (product!.Image != null)
            {
                var path = Path.Combine(_webHostEnvironment.WebRootPath, @"admin\images\products", product.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            _context.Products.Remove(product!);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "product", new { area = "admin" });
        }
        #endregion Xóa sản phẩm
    }
}