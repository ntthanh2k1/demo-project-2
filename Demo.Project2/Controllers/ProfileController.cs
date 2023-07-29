using Demo.Project2.Context;
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
    }
}