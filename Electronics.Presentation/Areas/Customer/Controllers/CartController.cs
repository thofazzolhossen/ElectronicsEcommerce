using Electronics.Application.Product;
using Electronics.Presentation.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Electronics.Presentation.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        public CartController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var product = await _productService.GetByIdAsync(productId);
            if (product == null) return NotFound();

            var cart = HttpContext.Session.Get<List<ProductDto>>("Cart") ?? new List<ProductDto>();

            var existingProduct = cart.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Quantity += quantity;
            }
            else
            {
                product.Quantity = quantity;
                cart.Add(product);
            }

            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<ProductDto>>("Cart") ?? new List<ProductDto>();
            return View(cart);
        }
    }
}
