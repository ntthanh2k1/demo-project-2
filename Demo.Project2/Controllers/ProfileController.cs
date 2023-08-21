using Demo.Project2.Context;
using Demo.Project2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Demo.Project2.Controllers
{
    [Authorize(Roles = "Customer", AuthenticationSchemes = "CustomerSchemes")]
    [Route("profile")]
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
            var user = await _context.Users!.FirstOrDefaultAsync(a => a.Id.Equals(Guid.Parse(id)));
            return View(user);
        }
        #endregion Trang thông tin cá nhân

        #region Cập nhật thông tin cá nhân
        [HttpGet]
        [Route("edit")]
        public async Task<IActionResult> Edit()
        {
            var id = User.FindFirstValue(ClaimTypes.Sid);
            var user = await _context.Users!.FindAsync(Guid.Parse(id));
            return View("edit", user);
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(User user)
        {
            var id = User.FindFirstValue(ClaimTypes.Sid);
            var currentUser = await _context.Users!.FindAsync(Guid.Parse(id));
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
            var user = await _context.Users!.FindAsync(Guid.Parse(id));
            return View("editPassword", user);
        }

        [HttpPost]
        [Route("editPassword")]
        public async Task<IActionResult> EditPassword(User user)
        {
            var id = User.FindFirstValue(ClaimTypes.Sid);
            var currentUser = await _context.Users!.FindAsync(Guid.Parse(id));
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

        #region Danh sách lịch sử đơn hàng
        [HttpGet]
        [Route("orderHistory")]
        public async Task<IActionResult> OrderHistory()
        {
            var id = User.FindFirstValue(ClaimTypes.Sid);
            var orders = await _context.Orders!
                .Where(a => a.UserId.Equals(Guid.Parse(id)))
                .OrderByDescending(a => a.CreatedOn)
                .ToListAsync();
            return View(orders);
        }
        #endregion Danh sách lịch sử đơn hàng

        #region Xem chi tiết lịch sử đơn hàng
        [HttpGet]
        [Route("orderHistoryDetails/{id}")]
        public async Task<IActionResult> OrderHistoryDetails(Guid id)
        {
            var order = await _context.Orders!.FirstOrDefaultAsync(a => a.Id.Equals(id));
            return View("orderHistoryDetails", order);
        }
        #endregion Xem chi tiết lịch sử đơn hàng
    }
}