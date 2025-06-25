using Electronics.Application.User;
using Electronics.Presentation.Areas.Admin.Views.User;
using Microsoft.AspNetCore.Mvc;

namespace Electronics.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllUsersAsync();

            var viewModel = new UserViewModel
            {
                Users = users
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new CreateUserDto
            {
                FullName = model.FullName,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Password = model.Password
            };

            var success = await _userService.CreateUserAsync(dto);
            if (success)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "User creation failed.");
            return View(model);
        }

    }
}
