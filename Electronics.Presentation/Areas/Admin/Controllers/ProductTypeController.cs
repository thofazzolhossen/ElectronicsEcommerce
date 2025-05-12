using Electronics.Application.ProductType;
using Electronics.Presentation.Areas.Admin.Views.ProductType;
using Microsoft.AspNetCore.Mvc;

namespace Electronics.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypeController : Controller
    {
        private readonly IProductTypeService _service;
        public ProductTypeController(IProductTypeService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var productTypes = await _service.GetAllAsync();
            var vm = new ProductTypeIndexVM
            {
                ProductTypes = productTypes.ToList()
            };

            return View(vm);
           
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductTypeDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.AddAsync(dto);
                return RedirectToAction("Index");
            }
            return View(dto);
        }



        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null) return NotFound();
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductTypeDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateAsync(dto);
                return RedirectToAction("Index");
            }
            return View(dto);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }

    }
}
