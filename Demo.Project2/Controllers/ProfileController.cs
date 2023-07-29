using Microsoft.AspNetCore.Mvc;

namespace Demo.Project2.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
