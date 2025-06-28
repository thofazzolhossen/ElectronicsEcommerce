using Electronics.Application.ProductTag;
using Electronics.Presentation.Areas.Admin.Views.ProductTag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Electronics.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductTagController : Controller
    {
        private readonly IProductTagService _service;
        public ProductTagController(IProductTagService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var ProductTag = await _service.GetAllAsync();
            var vm = new ProductTagIndexVM
            {
                ProductTags = ProductTag.ToList()
            };
            return View(vm);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductTagDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.AddAsync(dto);
                return RedirectToAction("Index");
            }
            return View(dto);
        }
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var dto = await _service.GetByIdAsync(id);
            if (dto == null)
            {
                return NotFound();
            }
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult>Edit(ProductTagDto dto)
        {
            if(ModelState.IsValid)
            {
                await _service.UpdateAsync(dto);
                return RedirectToAction("Index");
            }
            return View(dto);
        }

    }
}
