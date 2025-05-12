using Electronics.Application.Product;
using Electronics.Application.ProductTag;
using Electronics.Application.ProductType;
using Electronics.Application.Discount;
using Electronics.Presentation.Areas.Admin.Views.Product;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;

[Area("Admin")]
public class ProductController : Controller
{
    private readonly IProductService _service;
    private readonly IProductTypeService _productTypeService;
    private readonly IProductTagService _productTagService;
    private readonly IDiscountService _discountService;

    public ProductController(
        IProductService service,
        IProductTypeService productTypeService,
        IProductTagService productTagService,
        IDiscountService discountService)
    {
        _service = service;
        _productTypeService = productTypeService;
        _productTagService = productTagService;
        _discountService = discountService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _service.GetAllAsync();
        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        var vm = new ProductVM
        {
            Product = new ProductDto(),
            ProductTypes = await GetProductTypes(),
            ProductTags = await GetProductTags(),
            Discounts = await GetDiscounts()
        };
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductVM vm)
    {
        if (!ModelState.IsValid)
        {
            vm.ProductTypes = await GetProductTypes();
            vm.ProductTags = await GetProductTags();
            return View(vm);
        }

        if (vm.Product.DiscountId == 0 || vm.Product.DiscountId == null)
        {
            vm.Product.DiscountId = null;
        }

        await _service.AddAsync(vm.Product);
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Edit(int id)
    {
        var dto = await _service.GetByIdAsync(id);
        if (dto == null) return NotFound();

        var vm = new ProductVM
        {
            Product = dto,
            ProductTypes = await GetProductTypes(),
            ProductTags = await GetProductTags(),
            Discounts = await GetDiscounts()
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductVM vm)
    {
        if (!ModelState.IsValid)
        {
            vm.ProductTypes = await GetProductTypes();
            vm.ProductTags = await GetProductTags();
            vm.Discounts = await GetDiscounts();
            return View(vm);
        }

        await _service.UpdateAsync(vm.Product);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }

    // Helpers
    private async Task<IEnumerable<SelectListItem>> GetProductTypes()
    {
        var types = await _productTypeService.GetAllAsync();
        return types.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name
        });
    }

    private async Task<IEnumerable<SelectListItem>> GetProductTags()
    {
        var tags = await _productTagService.GetAllAsync();
        return tags.Select(t => new SelectListItem
        {
            Value = t.Id.ToString(),
            Text = t.Name
        });
    }

    private async Task<IEnumerable<SelectListItem>> GetDiscounts()
    {
        var discounts = await _discountService.GetAllAsync();
        return discounts.Select(d => new SelectListItem
        {
            Value = d.Id.ToString(),
            Text = d.Name
        });
    }
}
