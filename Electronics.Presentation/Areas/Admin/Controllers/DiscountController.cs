using Electronics.Application.Discount;
using Electronics.Application.ProductType;
using Electronics.Presentation.Areas.Admin.Views.Discount;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Electronics.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DiscountController : Controller
    {
        private readonly IDiscountService _service;
        public DiscountController(IDiscountService service)
        {
            _service = service;
        }
        public async Task<IActionResult> Index()
        {
            var discount = await _service.GetAllAsync();
            var vm = new DiscountVM
            {
                AllDiscount = discount.ToList()
            };
            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(DiscountDto dto)
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
        public async Task<IActionResult> Edit(DiscountDto dto)
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
