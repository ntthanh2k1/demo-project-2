using Demo.Project2.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Project2.ViewComponents
{
    public class SearchViewComponent : ViewComponent
    {
        private readonly DemoProject2DbContext _context;

        public SearchViewComponent(DemoProject2DbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            return View("index");
        }
    }
}
