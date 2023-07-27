using Demo.Project2.Context;
using Demo.Project2.Helper;
using Demo.Project2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace Demo.Project2.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly DemoProject2DbContext _context;
        private readonly AuthHelper _authHelper = new();

        public AuthController(DemoProject2DbContext context)
        {
            _context = context;
        }

        #region Đăng ký
        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(User user)
        {
            var newUser = new User();
            var existedUser = await _context.Users!.AnyAsync(a => a.Username!.Equals(user.Username));
            if (existedUser)
            {
                ViewBag.Error = "Tài khoản đã tồn tại.";
                return View("register", newUser);
            }
            if (user.Password!.Length < 6)
            {
                ViewBag.Error = "Mật khẩu phải có ít nhất 6 ký tự.";
                return View("register", newUser);
            }
            newUser.Username = user.Username;
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            newUser.FullName = user.FullName;
            newUser.Email = user.Email;
            newUser.IsActive = true;
            _context.Add(newUser);
            var customerRole = await _context.Roles!.FirstOrDefaultAsync(a => a.Code!.Equals("CUSTOMER"));
            var newUserRole = new UserRole
            {
                UserId = newUser.Id,
                RoleId = customerRole!.Id,
                IsActive = true
            };
            _context.Add(newUserRole);
            await _context.SaveChangesAsync();
            ViewBag.Success = "Đăng ký thành công.";
            return View("register", newUser);
        }
        #endregion Đăng ký

        #region Đăng nhập
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _context.Users!
                .FirstOrDefaultAsync(a => a.Username!.Equals(username) && a.IsActive);
            if (user == null)
            {
                ViewBag.Error = "Tài khoản không hợp lệ.";
                return View("login", user);
            }
            var userRole = user.UserRoles
                .FirstOrDefault(a => a.RoleId.Equals(Guid.Parse("df07ec54-06e2-4646-a767-98d36924ef7f")) && a.IsActive);
            if (userRole == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                ViewBag.Error = "Tài khoản không hợp lệ";
                return View("login", user);
            }
            await _authHelper.Login(HttpContext, user, "CustomerSchemes");
            return RedirectToAction("index", "home");
        }
        #endregion Đăng nhập

        #region Đăng xuất
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authHelper.Logout(HttpContext, "CustomerSchemes");
            return RedirectToAction("index", "home");
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
