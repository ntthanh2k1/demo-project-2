using Demo.Project2.Dtos;
using Demo.Project2.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Project2.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Item>? cart = SessionHelper.Get<List<Item>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                ViewBag.CountItems = 0;
            }
            else
            {
                ViewBag.CountItems = cart.Count;
            }
            return View("index");
        }
    }
}
