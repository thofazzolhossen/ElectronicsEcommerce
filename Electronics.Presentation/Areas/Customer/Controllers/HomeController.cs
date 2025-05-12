using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Electronics.Presentation.Models;
using Electronics.Application.Product;
using Electronics.Application.ProductImage;

namespace Electronics.Presentation.Areas.Customer.Controllers;

[Area("Customer")]
public class HomeController : Controller
{
    private readonly IProductService _productService;
    

    public HomeController(IProductService productService)
    {
        _productService = productService;
        
    }



    public async Task<IActionResult> Index()
    {
        // Fetch all products from the product service
        var products = await _productService.GetAllAsync();
        return View(products);
    }
    public async Task<IActionResult> AllProducts(string? type)
    {
        var products = await _productService.GetAllAsync(); // Make sure this returns ProductDto list

        if (!string.IsNullOrEmpty(type))
        {
            products = products.Where(p => p.ProductTypeName == type).ToList();
            ViewBag.FilteredType = type;
        }

        return View(products);
    }

    public async Task<IActionResult> Details(int id)
    {
        
        var product = await _productService.GetByIdAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        return View(product);
    }



}
