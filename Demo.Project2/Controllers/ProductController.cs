using Demo.Project2.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Project2.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly DemoProject2DbContext _context;

        public ProductController(DemoProject2DbContext context)
        {
            _context = context;
        }

        #region Xem sản phẩm theo phân loại
        [HttpGet]
        [Route("getProductsByCategory/{id}")]
        public  async Task<IActionResult> GetProductsByCategory(Guid id)
        {
            var products = await _context.Products!
                .Where(a => a.CategoryId.Equals(id) && a.IsActive)
                .ToListAsync();
            return View("GetProductsByCategory", products);
        }
        #endregion Xem sản phẩm theo phân loại

        #region Xem chi tiết sản phẩm
        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _context.Products!.FirstOrDefaultAsync(a => a.Id.Equals(id));
            return View("details", product);
        }
        #endregion Xem chi tiết sản phẩm
    }
}