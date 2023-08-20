using Demo.Project2.Context;
using Demo.Project2.Dtos;
using Demo.Project2.Helper;
using Demo.Project2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ML;
using System.Data;
using System.Security.Claims;
using static Demo_Project2.SentimentAnalysis;

namespace Demo.Project2.Controllers
{
    [Route("product")]
    public class ProductController : Controller
    {
        private readonly DemoProject2DbContext _context;
        private readonly PredictionEnginePool<ModelInput, ModelOutput> _predictionEnginePool;

        public ProductController(DemoProject2DbContext context, PredictionEnginePool<ModelInput, ModelOutput> predictionEnginePool)
        {
            _context = context;
            _predictionEnginePool = predictionEnginePool;
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
            if (quantity <= 0)
            {
                return RedirectToAction("details", product);
            }
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
                    cart![index].Quantity += quantity;
                }
                SessionHelper.Set(HttpContext.Session, "cart", cart);
            }
            return RedirectToAction("details", product);
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

        #region Bình luận sản phẩm
        [Authorize(Roles = "Customer", AuthenticationSchemes = "CustomerSchemes")]
        [HttpPost]
        [Route("addReview")]
        public async Task<IActionResult> AddReview(Guid id, string text)
        {
            var product = await _context.Products!.FindAsync(id);
            var userId = User.FindFirstValue(ClaimTypes.Sid);
            var username = User.FindFirstValue(ClaimTypes.Name);
            var newReview = new Review
            {
                UserId = Guid.Parse(userId),
                ProductId = product!.Id,
                Username = username,
                Text = text,
                CreatedOn = DateTime.Now
            };
            var input = new ModelInput
            {
                Content = text
            };
            var prediction = _predictionEnginePool.Predict(input);
            string? sentiment;
            if (prediction.PredictedLabel.Equals("POS"))
            {
                sentiment = "Tích cực";
            }
            else if (prediction.PredictedLabel.Equals("NEG"))
            {
                sentiment = "Tiêu cực";
            }
            else
            {
                sentiment = "Bình thường";
            }
            newReview.Sentiment = sentiment;
            _context.Add(newReview);
            await _context.SaveChangesAsync();
            return RedirectToAction("details", product);
        }
        #endregion Bình luận sản phẩm
    }
}