using Demo.Project2.Context;
using Demo.Project2.Helper;
using Demo.Project2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Project2.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/auth")]
    public class AuthController : Controller
    {
        private readonly DemoProject2DbContext _context;
        private readonly SecurityHelper _securityHelper = new();

        public AuthController(DemoProject2DbContext context)
        {
            _context = context;
        }

        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(a => a.Username.Equals(username) && a.Status == true);
            if (user != null)
            {
                var userRole = user.UserRoles.FirstOrDefault(a => a.RoleId == 1 && a.Status == true);
                if (userRole != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    await _securityHelper.SignIn(this.HttpContext, user, "AdminSchemes");
                    return RedirectToAction("index", "home", new { area = "admin" });
                }
            }
            ViewBag.error = "Tài khoản không hợp lệ";
            return View("Index");
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _securityHelper.SignOut(this.HttpContext, "AdminSchemes");
            return RedirectToAction("index", "auth", new { area = "admin" });
        }

        [Route("accessDenied")]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}
