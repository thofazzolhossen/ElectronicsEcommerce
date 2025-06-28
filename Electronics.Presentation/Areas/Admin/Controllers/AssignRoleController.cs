using Electronics.Application.AssignRole;
using Electronics.Presentation.Areas.Admin.Views.AssignRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Electronics.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AssignRoleController : Controller
    {
        private readonly IUserRoleService _service;     

        public AssignRoleController(IUserRoleService service)
        {
            _service = service;
        }

        public async Task<IActionResult> AssignRole()
        {
            var users = await _service.GetUsersAsync();
            var roles = await _service.GetRolesAsync();

            var vm = new AssignRoleViewModel
            {
                Users = users.Select(u => new SelectListItem { Value = u.Id, Text = u.Name }).ToList(),
                Roles = roles.Select(r => new SelectListItem { Value = r, Text = r }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRoleViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var users = await _service.GetUsersAsync();
                var roles = await _service.GetRolesAsync();

                vm.Users = users.Select(u => new SelectListItem { Value = u.Id, Text = u.Name }).ToList();
                vm.Roles = roles.Select(r => new SelectListItem { Value = r, Text = r }).ToList();

                return View(vm);
            }

            var dto = new AssignRoleDto
            {
                UserId = vm.SelectedUserId,
                RoleName = vm.SelectedRole
            };

            var result = await _service.AssignRoleAsync(dto);

            if (result)
            {
                TempData["Success"] = "Role assigned successfully.";
                return RedirectToAction("AssignRole");
            }
            else
            {
                TempData["Error"] = "Failed to assign role.";
                return View(vm);
            }
        }
    }
}
