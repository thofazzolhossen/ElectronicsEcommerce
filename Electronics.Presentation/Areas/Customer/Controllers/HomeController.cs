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
        var products = await _productService.GetAllAsync();
        return View(products);
    }
    public async Task<IActionResult> AllProducts(string? type)
    {
        var products = await _productService.GetAllAsync();

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
    public async Task<IActionResult> OthersDetails(int id)
    {

        var product = await _productService.GetByIdAsync(id);
        var allProducts = await _productService.GetAllAsync();
        if (product == null)
        {
            return NotFound();
        }
        var withoutSpecificIdTypeProduct = allProducts
        //p => p.ProductTypeId != product.ProductTypeId
        .Where(p => p.ProductTypeId == product.ProductTypeId &&  product.Id != p.Id)
        .ToList();

        ViewBag.WithoutSpecificIdTypeProduct = withoutSpecificIdTypeProduct;

        return View(product);

    }



}
