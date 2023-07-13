using Demo.Project2.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Project2.ViewComponents
{
    public class SlideViewComponent : ViewComponent
    {
        private readonly DemoProject2DbContext _context;

        public SlideViewComponent(DemoProject2DbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var slides = await _context.Slides!.Where(a => a.IsActive == true).Take(1).ToListAsync();
            return View("index", slides);
        }
    }
}
