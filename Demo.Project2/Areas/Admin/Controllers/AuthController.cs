using Demo.Project2.Context;
using Demo.Project2.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Project2.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/auth")]
    public class AuthController : Controller
    {
        private readonly DemoProject2DbContext _context;
        private readonly AuthHelper _authHelper = new();

        public AuthController(DemoProject2DbContext context)
        {
            _context = context;
        }

        #region Trang đăng nhập
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        #endregion Trang đăng nhập

        #region Đăng nhập
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users!
                .FirstOrDefaultAsync(a => a.Username!.Equals(username) && a.IsActive == true);
            if (user == null)
            {
                ViewBag.error = "Tài khoản không hợp lệ.";
                return View("index");
            }
            var userRole = user.UserRoles
                .FirstOrDefault(a => a.RoleId.Equals(Guid.Parse("93312dfb-7580-4946-ab83-4f23ba834a28")) && a.IsActive == true);
            if (userRole == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                ViewBag.error = "Tài khoản không hợp lệ";
                return View("index");
            }
            await _authHelper.Login(HttpContext, user, "AdminSchemes");
            return RedirectToAction("index", "home", new { area = "admin" });
        }
        #endregion Đăng nhập

        #region Đăng xuất
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authHelper.Logout(HttpContext, "AdminSchemes");
            return RedirectToAction("index", "auth", new { area = "admin" });
        }
        #endregion Đăng xuất

        #region Không cho truy cập
        [Route("accessDenied")]
        public IActionResult AccessDenied()
        {
            return View("accessDenied");
        }
        #endregion Không cho truy cập
    }
}