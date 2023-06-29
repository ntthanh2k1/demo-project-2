using Demo.Project2.Context;
using Demo.Project2.Helper;
using Demo.Project2.Models;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Project2.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/auth")]
    public class AuthController : Controller
    {
        private readonly DemoProject2DbContext _context;
        private readonly SecurityHelper _securityHelper = new SecurityHelper();

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
        public IActionResult Login(string username, string password)
        {
            var user = Authenticate(username, password);
            if (user != null)
            {
                _securityHelper.SignIn(this.HttpContext, user, "AdminSchemes");
                return RedirectToAction("index", "dashboard", new { area = "admin" });
            }
            else
            {
                ViewBag.error = "Tài khoản không hợp lệ";
                return View("Index");
            }
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            _securityHelper.SignOut(this.HttpContext, "AdminSchemes");
            return RedirectToAction("index", "auth", new { area = "admin" });
        }

        [Route("accessDenied")]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }

        private User Authenticate(string username, string password)
        {
            var account = _context.Users.SingleOrDefault(a => a.Username.Equals(username) && a.Status == true);
            if (account != null)
            {
                var roleOfAccount = account.UserRoles.FirstOrDefault();
                if (roleOfAccount?.RoleId == 1 && BCrypt.Net.BCrypt.Verify(password, account.Password))
                {
                    return account;
                }
            }
            return null;
        }
    }
}
