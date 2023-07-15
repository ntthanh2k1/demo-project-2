using Demo.Project2.Context;
using Demo.Project2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Demo.Project2.Controllers
{
    public class HomeController : Controller
    {
        private readonly DemoProject2DbContext _context;

        public HomeController(DemoProject2DbContext context)
        {
            _context = context;
        }

        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products!.Where(a => a.IsActive).ToListAsync();
            return View(products);
        }
    }
}