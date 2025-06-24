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
    }
}
