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
        private readonly EmailService _emailService;
        public CartController(IProductService productService, AppDbContext db, IProductImageService productImageService, EmailService emailService)
        {
            _productService = productService;
            _db = db;
            _productImageService = productImageService;
            _emailService = emailService;
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
                            detail.Quantity = match.Quantity;
                        }
                    }
                }
            }

            return View(orders);
        }
        [HttpPost]
        public async Task<IActionResult> MarkAsDone(int productId)
        {
            // Step 1: Find the OrderDetail and include Order
            var orderDetail = await _db.OrderDetails
                .Include(od => od.Order)
                .FirstOrDefaultAsync(od => od.ProductId == productId);

            if (orderDetail == null)
                return NotFound();

            // Step 2: Load the full Order with all OrderDetails and Products
            var order = await _db.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == orderDetail.OrderId);

            if (order == null || string.IsNullOrEmpty(order.Email))
                return BadRequest("Order or Email not found.");

            // Step 3: Build Email Body
            string subject = "Order Completed";
            string body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Order Completed</title>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f8f9fa;
            padding: 20px;
        }}
        .container {{
            max-width: 600px;
            margin: auto;
            background-color: #ffffff;
            border-radius: 8px;
            border: 1px solid #dee2e6;
            padding: 30px;
            box-shadow: 0 0 10px rgba(0,0,0,0.1);
        }}
        .header {{
            background-color: #28a745;
            color: white;
            padding: 15px;
            border-radius: 6px 6px 0 0;
            text-align: center;
            font-size: 22px;
            font-weight: bold;
        }}
        .table {{
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }}
        .table th, .table td {{
            border: 1px solid #dee2e6;
            padding: 10px;
            text-align: left;
        }}
        .table th {{
            background-color: #f1f1f1;
        }}
        .footer {{
            margin-top: 30px;
            font-size: 14px;
            color: #6c757d;
            text-align: center;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>🎉 Order Completed Successfully!</div>
        <p>Dear <strong>{order.FirstName}</strong>,</p>
        <p>Your order has been <span style='color:green;'>successfully completed</span>.</p>
        <table class='table'>
            <thead>
                <tr>
                    <th>Product Name</th>
                    <th>Quantity</th>
                    <th>Price (৳)</th>
                </tr>
            </thead>
            <tbody>";

            // Step 4: Loop through OrderDetails to add rows
            foreach (var item in order.OrderDetails)
            {
                body += $@"
                <tr>
                    <td>{item.Product?.Name}</td>
                    <td>{item.Quantity}</td>
                    <td>{item.Product?.Price.ToString("0.00")}</td>
                </tr>";
            }

            // Step 5: Close HTML
            body += @"
            </tbody>
        </table>
        <p style='margin-top: 20px;'>We appreciate your trust in us. 🌟<br>
        Stay tuned for exciting new deals and products!</p>

        <p>Warm regards,<br><strong>TechHutBD.com</strong></p>

        <div class='footer'>
            Need help? Contact our support team anytime.<br />
            📧 techhutbd.com@gmail.com | 📞 01712-511701
        </div>
    </div>
</body>
</html>";

            // Step 6: Send Email
            await _emailService.SendEmailAsync(order.Email, subject, body);
            
            _db.OrderDetails.RemoveRange(order.OrderDetails);
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Order marked as done and email sent successfully.";
            return RedirectToAction("AllOrders");
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsCancel(int productId)
        {
            // অর্ডার ডিটেইল নিয়ে আসা
            var orderDetail = await _db.OrderDetails
                .Include(od => od.Order)
                .FirstOrDefaultAsync(od => od.ProductId == productId);

            if (orderDetail == null)
                return NotFound();

            // পুরো অর্ডার লোড করা (OrderDetails ও Products সহ)
            var order = await _db.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == orderDetail.OrderId);

            if (order == null || string.IsNullOrEmpty(order.Email))
                return BadRequest("Order or Email not found.");

            // মেইল তৈরি করা
            string subject = "Order Cancelled";
            string body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <title>Order Cancelled</title>
    <style>
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #fff0f0;
            padding: 20px;
        }}
        .container {{
            max-width: 600px;
            margin: auto;
            background-color: #ffffff;
            border-radius: 8px;
            border: 1px solid #f5c6cb;
            padding: 30px;
            box-shadow: 0 0 10px rgba(255, 0, 0, 0.2);
        }}
        .header {{
            background-color: #dc3545;
            color: white;
            padding: 15px;
            border-radius: 6px 6px 0 0;
            text-align: center;
            font-size: 22px;
            font-weight: bold;
        }}
        .table {{
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }}
        .table th, .table td {{
            border: 1px solid #dee2e6;
            padding: 10px;
            text-align: left;
        }}
        .table th {{
            background-color: #f8d7da;
        }}
        .footer {{
            margin-top: 30px;
            font-size: 14px;
            color: #6c757d;
            text-align: center;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>❌ Order Cancelled</div>
        <p>Dear <strong>{order.FirstName}</strong>,</p>
        <p>We regret to inform you that your order <strong>#{order.OrderNumber}</strong> has been <span style='color:red;'>cancelled</span>.</p>
        <p>If you have any questions, please contact our support team.</p>
        <table class='table'>
            <thead>
                <tr>
                    <th>Product Name</th>
                    <th>Quantity</th>
                    <th>Price (৳)</th>
                </tr>
            </thead>
            <tbody>";

            foreach (var item in order.OrderDetails)
            {
                body += $@"
                <tr>
                    <td>{item.Product?.Name}</td>
                    <td>{item.Quantity}</td>
                    <td>{item.Product?.Price.ToString("0.00")}</td>
                </tr>";
            }

            body += @"
            </tbody>
        </table>

        <p style='margin-top: 20px;'>Thank you for understanding.<br>
        We hope to serve you better in the future.</p>

        <p>Best regards,<br><strong>TechHutBD.com</strong></p>

        <div class='footer'>
            Need help? Contact our support team anytime.<br />
            📧 techhutbd.com@gmail.com | 📞 01712-511701
        </div>
    </div>
</body>
</html>";

            await _emailService.SendEmailAsync(order.Email, subject, body);
            _db.OrderDetails.RemoveRange(order.OrderDetails);
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();

            TempData["Success"] = "Order cancelled and email sent successfully.";
            return RedirectToAction("AllOrders");
        }




    }
}
