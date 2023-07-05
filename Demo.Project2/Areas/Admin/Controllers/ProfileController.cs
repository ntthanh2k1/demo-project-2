using Demo.Project2.Context;
using Demo.Project2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;

namespace Demo.Project2.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminSchemes")]
    [Area("admin")]
    [Route("admin/profile")]
    public class ProfileController : Controller
    {
        private readonly DemoProject2DbContext _context;

        public ProfileController(DemoProject2DbContext context)
        {
            _context = context;
        }

        #region Trang thông tin cá nhân
        [HttpGet]
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Username.Equals(username));
            return View(user);
        }
        #endregion Trang thông tin cá nhân

        #region Cập nhật thông tin cá nhân
        [HttpGet]
        [Route("update")]
        public async Task<IActionResult> Update()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Username.Equals(username));
            return View("update", user);
        }

        [HttpPost]
        [Route("update")]
        public async Task<IActionResult> Update(User user)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(a => a.Id == user.Id);
            currentUser.FullName = user.FullName;
            currentUser.Email = user.Email;
            _context.Update(currentUser);
            await _context.SaveChangesAsync();
            ViewBag.Success = "Cập nhật thành công.";
            return View("Update", currentUser);
        }
        #endregion Cập nhật thông tin cá nhân

        #region Đổi mật khẩu
        [HttpGet]
        [Route("updatePassword")]
        public async Task<IActionResult> UpdatePassword()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Username.Equals(username));
            return View("updatePassword", user);
        }

        [HttpPost]
        [Route("updatePassword")]
        public async Task<IActionResult> UpdatePassword(User user)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(a => a.Id == user.Id);
            if (user.Password.Length < 6)
            {
                ViewBag.Error = "Mật khẩu it nhất từ 6 ký tự trở lên.";
                return View("UpdatePassword", currentUser);
            }
            currentUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Update(currentUser);
            await _context.SaveChangesAsync();
            ViewBag.Success = "Cập nhật thành công.";
            return View("UpdatePassword", currentUser);
        }
        #endregion Đổi mật khẩu
    }
}
