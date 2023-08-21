using Demo.Project2.Context;
using Demo.Project2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Project2.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin", AuthenticationSchemes = "AdminSchemes")]
    [Area("admin")]
    [Route("admin/order")]
    public class OrderController : Controller
    {
        private readonly DemoProject2DbContext _context;

        public OrderController(DemoProject2DbContext context)
        {
            _context = context;
        }

        #region Danh sách đơn hàng
        [HttpGet]
        [Route("")]
        [Route("index")]
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders!.OrderByDescending(a => a.CreatedOn).ToListAsync();
            return View(orders);
        }
        #endregion Danh sách đơn hàng

        #region Xem chi tiết đơn hàng
        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var order = await _context.Orders!.FirstOrDefaultAsync(a => a.Id.Equals(id));
            return View("details", order);
        }
        #endregion Xem chi tiết đơn hàng

        #region Hoàn tất đơn hàng
        [HttpGet]
        [Route("completedOrder/{id}")]
        public async Task<IActionResult> CompletedOrder(Guid id)
        {
            var currentOrder = await _context.Orders!.FindAsync(id);
            currentOrder!.OrderStatus = OrderStatus.Completed;
            _context.Update(currentOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "order", new { area = "admin" });
        }
        #endregion Hoàn tất đơn hàng

        #region Hủy đơn hàng
        [HttpGet]
        [Route("cancelledOrder/{id}")]
        public async Task<IActionResult> CancelledOrder(Guid id)
        {
            var currentOrder = await _context.Orders!.FindAsync(id);
            currentOrder!.OrderStatus = OrderStatus.Cancelled;
            _context.Update(currentOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "order", new { area = "admin" });
        }
        #endregion Hủy đơn hàng

        #region Xóa đơn hàng
        [HttpGet]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var order = await _context.Orders!.FindAsync(id);
            _context.OrderDetails!.RemoveRange(order!.OrderDetails);
            _context.Orders.Remove(order!);
            await _context.SaveChangesAsync();
            return RedirectToAction("index", "order", new { area = "admin" });
        }
        #endregion Xóa đơn hàng
    }
}
