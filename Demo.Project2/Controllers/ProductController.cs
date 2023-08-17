using Demo.Project2.Context;
using Demo.Project2.Dtos;
using Demo.Project2.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.Project2.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly DemoProject2DbContext _context;

        public ProductController(DemoProject2DbContext context)
        {
            _context = context;
        }

        #region Xem sản phẩm theo phân loại
        [HttpGet]
        [Route("getProductsByCategory/{id}")]
        public  async Task<IActionResult> GetProductsByCategory(Guid id)
        {
            var products = await _context.Products!
                .Where(a => a.CategoryId.Equals(id) && a.IsActive)
                .ToListAsync();
            return View("GetProductsByCategory", products);
        }
        #endregion Xem sản phẩm theo phân loại

        #region Xem chi tiết sản phẩm
        [HttpGet]
        [Route("details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            var product = await _context.Products!.FirstOrDefaultAsync(a => a.Id.Equals(id));
            return View("details", product);
        }
        #endregion Xem chi tiết sản phẩm

        #region Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        [Route("addToCart")]
        public async Task<IActionResult> AddToCart(Guid id, int quantity)
        {
            var product = await _context.Products!.FindAsync(id);
            var cart = SessionHelper.Get<List<Item>>(HttpContext.Session, "cart");
            if (cart == null)
            {
                cart = new List<Item>
                {
                    new Item
                    {
                        Id = product!.Id,
                        Name = product.Name,
                        Image = product.Image,
                        Price = product.Price,
                        Quantity = quantity
                    }
                };
                SessionHelper.Set(HttpContext.Session, "cart", cart);
            }
            else
            {
                int index = ItemExists(id, cart);
                if (index == -1)
                {
                    cart.Add(new Item
                    {
                        Id = product!.Id,
                        Name = product.Name,
                        Image = product.Image,
                        Price = product.Price,
                        Quantity = quantity
                    });
                }
                else
                {
                    cart![index].Quantity++;
                }
                SessionHelper.Set(HttpContext.Session, "cart", cart);
            }
            return View("details", product);
        }

        private static int ItemExists(Guid id, List<Item>? cart)
        {
            for (var i = 0; i < cart!.Count; i++)
            {
                if (cart[i].Id == id)
                {
                    return i;
                }
            }
            return -1;
        }
        #endregion Thêm sản phẩm vào giỏ hàng
    }
}