using Demo.Project2.Context;
using Demo.Project2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Demo.Project2.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly DemoProject2DbContext _context;

        public HomeController(DemoProject2DbContext context)
        {
            _context = context;
        }

        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products!
                .Where(a => a.IsFeatured && a.IsActive).Take(4).ToListAsync();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}