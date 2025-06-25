using Electronics.Application.Role;
using Electronics.Presentation.Areas.Admin.Views.Role;
using Microsoft.AspNetCore.Mvc;

namespace Electronics.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var roles = await _service.GetAllAsync();
            return View(roles);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _service.CreateAsync(model.Name);
            if (result) return RedirectToAction("Index");

            ModelState.AddModelError("", "Role already exists or failed.");
            return View(model);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _service.GetByIdAsync(id);
            if (role == null) return NotFound();

            return View(new EditRoleViewModel { Id = role.Id, Name = role.Name });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _service.UpdateAsync(model.Id, model.Name);
            if (result) return RedirectToAction("Index");

            ModelState.AddModelError("", "Failed to update role.");
            return View(model);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await _service.GetByIdAsync(id);
            if (role == null) return NotFound();

            return View(role);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var result = await _service.DeleteAsync(id);
            if (result)
            {
                TempData["Success"] = "Role deleted successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to delete role.";
            }

            return RedirectToAction("Index");
        }
    }
}
