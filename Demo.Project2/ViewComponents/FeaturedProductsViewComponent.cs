using Demo.Project2.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Project2.ViewComponents
{
    public class FeaturedProductsViewComponent : ViewComponent
    {
        private readonly DemoProject2DbContext _context;

        public FeaturedProductsViewComponent(DemoProject2DbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var featuredProducts = await _context.Products!
                .Where(a => a.IsFeatured && a.IsActive).Take(4).ToListAsync();
            return View("index", featuredProducts);
        }
    }
}
