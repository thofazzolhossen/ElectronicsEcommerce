using Electronics.Application.Product;
using Electronics.Application.ProductImage;
using Electronics.Domain.Entities;
using Electronics.Infrastructure;
using Electronics.Presentation.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Electronics.Presentation.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly AppDbContext _db;
        private readonly IProductImageService _productImageService;
        public CartController(IProductService productService, AppDbContext db, IProductImageService productImageService)
        {
            _productService = productService;
            _db = db;
            _productImageService = productImageService;
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

        public IActionResult Checkout()
        {
            var cart = HttpContext.Session.Get<List<ProductDto>>("Cart") ?? new List<ProductDto>();
            ViewBag.CartItems = cart;
            return View(new Order());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.Get<List<ProductDto>>("Cart") ?? new List<ProductDto>();

            if (order.OrderDetails == null)
                order.OrderDetails = new List<OrderDetails>();

            if (cart != null)
            {
                foreach (var item in cart)
                {
                    OrderDetails orderDetails = new OrderDetails();
                    orderDetails.ProductId = item.Id;
                    orderDetails.Quantity = item.Quantity;
                    order.OrderDetails.Add(orderDetails);
                }
            }
            order.OrderNumber = GetOrderNo();
            order.OrderDate = DateTime.Now;
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            HttpContext.Session.Remove("Cart");
            return RedirectToAction("Index");
        }

        public string GetOrderNo()
        {
            int rowCount = _db.Orders.ToList().Count() + 1;
            return rowCount.ToString("000");
        }

        public async Task<IActionResult> AllOrders()
        {
            var orders = await _db.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .ToListAsync();

            foreach (var order in orders)
            {
                foreach (var detail in order.OrderDetails)
                {
                    var image = await _db.ProductImages
                        .Where(pi => pi.ProductId == detail.ProductId)
                        .FirstOrDefaultAsync();

                    if (image != null && detail.Product != null)
                    {
                        detail.Product.ImagePath = image.ImagePath;
                    }
                }
            }

            var cart = HttpContext.Session.Get<List<ProductDto>>("Cart");

            if (cart != null)
            {
                foreach (var order in orders)
                {
                    foreach (var detail in order.OrderDetails)
                    {
                        var match = cart.FirstOrDefault(c => c.Id == detail.ProductId);
                        if (match != null)
                        {
                            detail.Quantity = match.Quantity; // ✅ Add quantity from cart if available
                        }
                    }
                }
            }

            return View(orders);
        }



    }
}
