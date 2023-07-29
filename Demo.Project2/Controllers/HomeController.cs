using Demo.Project2.Context;
using Demo.Project2.Helper;
using Demo.Project2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Project2.Controllers
{
    public class HomeController : Controller
    {
        private readonly DemoProject2DbContext _context;

        public HomeController(DemoProject2DbContext context)
        {
            _context = context;
        }

        #region Trang chủ
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index(
            string searchString,
            string currentFilter,
            int? pageNumber)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var query = _context.Products!.Where(a => a.IsActive);
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(a => a.Name!.Contains(searchString));
            }
            var products = await PaginatedListHelper<Product>
                .CreateAsync(query, pageNumber ?? 1, 3);
            return View(products);
        }
        #endregion Trang chủ
    }
}