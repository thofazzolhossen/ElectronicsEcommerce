using Electronics.Application.Product;
using Electronics.Application.ProductImage;
using Electronics.Presentation.Areas.Admin.Views.ProductImage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Electronics.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductImageController : Controller
    {
        private readonly IProductImageService _service;
        private readonly IProductService _productService;

        public ProductImageController(IProductImageService service, IProductService productService)
        {
            _service = service;
            _productService = productService;
        }

        public async Task<IActionResult> Create()
        {
            var products = await _productService.GetAllAsync();
            var vm = new ProductImageVM
            {
                ProductList = products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductImageVM vm)
        {
            if (!ModelState.IsValid)
            {
                var products = await _productService.GetAllAsync();
                vm.ProductList = products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();
                return View(vm);
            }

            var dto = new ProductImageDto
            {
                ProductId = vm.ProductId,
                Images = vm.Images
            };

            await _service.AddAsync(dto);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Index()
        {
            var dtos = await _service.GetAllAsync();
            var vms = dtos.Select(dto => new ProductImageVM
            {
                Id = dto.Id,
               
                ProductId = dto.ProductId,
                ImagePath = dto.ImagePath
            }).ToList();

            return View(vms);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var dtos = await _service.GetByIdAsync(id);
            var dto = dtos.FirstOrDefault();
            if (dto == null) return NotFound();

            var vm = new ProductImageVM
            {
                Id = dto.Id,
                ProductId = dto.ProductId,
                ImagePath = dto.ImagePath,
            };

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
