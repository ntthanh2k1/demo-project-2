using Demo.Project2.Context;
using Demo.Project2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Project2.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminSchemes")]
    [Area("admin")]
    [Route("admin/user")]
    public class UserController : Controller
    {
        private readonly DemoProject2DbContext _context;

        public UserController(DemoProject2DbContext context)
        {
            _context = context;
        }

        #region Trang quản lý tài khoản
        [HttpGet]
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users!.ToListAsync();
            return View(users);
        }
        #endregion Trang quản lý tài khoản

        #region Xem chi tiết tài khoản
        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _context.Users!.FirstOrDefaultAsync(a => a.Id.Equals(id));
            return View("details", user);
        }
        #endregion Xem chi tiết tài khoản

        #region Cập nhật tài khoản
        [HttpGet]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var user = await _context.Users!.FindAsync(id);
            return View("edit", user);
        }
        
        [HttpPost]
        [Route("edit/{id}")]
        public async Task<IActionResult> Edit(Guid id, User user)
        {
            var currentUser = await _context.Users!.FindAsync(id);
            currentUser!.FullName = user.FullName;
            currentUser.Email = user.Email;
            currentUser.IsActive = user.IsActive;
            _context.Update(currentUser);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "user", new { area = "admin" });
        }
        #endregion Cập nhật tài khoản

        #region Xóa tài khoản
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _context.Users!.FindAsync(id);
            _context.Users.Remove(user!);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "user", new { area = "admin" });
        }
        #endregion Xóa tài khoản
    }
}
