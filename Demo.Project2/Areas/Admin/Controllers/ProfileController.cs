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
            var id = User.FindFirstValue(ClaimTypes.Sid);
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id.Equals(Guid.Parse(id)));
            return View(user);
        }
        #endregion Trang thông tin cá nhân

        #region Cập nhật thông tin cá nhân
        [HttpGet]
        [Route("edit")]
        public async Task<IActionResult> Edit()
        {
            var id = User.FindFirstValue(ClaimTypes.Sid);
            var user = await _context.Users.FindAsync(Guid.Parse(id));
            return View("edit", user);
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(User user)
        {
            var id = User.FindFirstValue(ClaimTypes.Sid);
            var currentUser = await _context.Users.FindAsync(Guid.Parse(id));
            currentUser!.FullName = user.FullName;
            currentUser.Email = user.Email;
            _context.Update(currentUser);
            await _context.SaveChangesAsync();
            ViewBag.Success = "Cập nhật thành công.";
            return View("edit", currentUser);
        }
        #endregion Cập nhật thông tin cá nhân

        #region Đổi mật khẩu
        [HttpGet]
        [Route("editPassword")]
        public async Task<IActionResult> EditPassword()
        {
            var id = User.FindFirstValue(ClaimTypes.Sid);
            var user = await _context.Users.FindAsync(Guid.Parse(id));
            return View("editPassword", user);
        }

        [HttpPost]
        [Route("editPassword")]
        public async Task<IActionResult> EditPassword(User user)
        {
            var id = User.FindFirstValue(ClaimTypes.Sid);
            var currentUser = await _context.Users.FindAsync(Guid.Parse(id));
            if (user.Password!.Length < 6)
            {
                ViewBag.Error = "Mật khẩu it nhất từ 6 ký tự trở lên.";
                return View("editPassword", currentUser);
            }
            currentUser!.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _context.Update(currentUser);
            await _context.SaveChangesAsync();
            ViewBag.Success = "Cập nhật thành công.";
            return View("editPassword", currentUser);
        }
        #endregion Đổi mật khẩu
    }
}
