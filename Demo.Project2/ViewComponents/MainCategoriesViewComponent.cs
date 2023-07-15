using Demo.Project2.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Project2.ViewComponents
{
    public class MainCategoriesViewComponent : ViewComponent
    {
        private readonly DemoProject2DbContext _context;

        public MainCategoriesViewComponent(DemoProject2DbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Categories!
                .Where(a => 
                    a.IsActive &&
                    a.ParentCategory == null &&
                    a.ChildCategories != null &&
                    a.ChildCategories.Count > 0)
                .ToListAsync();
            return View("index", categories);
        }
    }
}