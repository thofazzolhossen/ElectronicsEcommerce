using Electronics.Domain.Entities.Auth;
using Electronics.Presentation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Electronics.Presentation.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly SignInManager<Users> signInManager;
        private readonly UserManager<Users> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(SignInManager<Users> signInManager, UserManager<Users> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            return View(user);
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    TempData["Success"] = "Login successfully!";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email or password is incorrect.");
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Users users = new Users
                {
                    FullName = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                };

                var result = await userManager.CreateAsync(users, model.Password);

                if (result.Succeeded)
                {
                    string selectedRole = model.Role ?? "User";

                    if (!await roleManager.RoleExistsAsync(selectedRole))
                    {
                        await roleManager.CreateAsync(new IdentityRole(selectedRole));
                    }

                    await userManager.AddToRoleAsync(users, selectedRole);
                    TempData["Success"] = "Registration successfully done!";
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return View(model);
        }
        [AllowAnonymous]
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "Something is wrong!");
                    return View(model);
                }
                else
                {
                    TempData["Success"] = "Your email verified successfully!";
                    return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                }
            }
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel { Email = username });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var result = await userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await userManager.AddPasswordAsync(user, model.NewPassword);
                        TempData["Success"] = "Your password successfully!";
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email not found!");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong. try again.");
                return View(model);
            }
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            TempData["Success"] = "Logout successfully!";
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> UserList()
        {
            var users = userManager.Users.ToList();
            var userList = new List<UserListViewModel>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userList.Add(new UserListViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Roles = roles
                });
            }

            return View(userList);
        }

        // GET: Show the add role form
        public IActionResult AddRole()
        {
            return View();
        }


        // POST: Add the role to the system
        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                ModelState.AddModelError("", "Role name cannot be empty.");
                return View();
            }

            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                {
                    ViewBag.Message = $"Role '{roleName}' has been added successfully.";
                    TempData["Success"] = "User role add successfully!";
                    return View();
                }
                else
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                    return View();
                }
            }
            else
            {
                ModelState.AddModelError("", "Role already exists.");
                return View();
            }
        }

        public IActionResult RoleIndex()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }

        public async Task<IActionResult> AssignRole(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var roles = roleManager.Roles.Select(r => r.Name).ToList();
            var currentRoles = await userManager.GetRolesAsync(user);

            var model = new AssignRoleViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                Roles = roles,
                SelectedRole = currentRoles.FirstOrDefault()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRoleViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null) return NotFound();

            var currentRoles = await userManager.GetRolesAsync(user);
            await userManager.RemoveFromRolesAsync(user, currentRoles);
            await userManager.AddToRoleAsync(user, model.SelectedRole);

            TempData["Success"] = "Role assigned successfully!";
            return RedirectToAction("UserList");
        }
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            var model = new IdentityRole
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditRole(IdentityRole model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);
            if (role == null)
                return NotFound();

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                ModelState.AddModelError("", "Role name cannot be empty.");
                return View(model);
            }

            // Check if a different role with the same name exists
            var existingRole = await roleManager.FindByNameAsync(model.Name);
            if (existingRole != null && existingRole.Id != role.Id)
            {
                ModelState.AddModelError("", "Another role with the same name already exists.");
                return View(model);
            }

            role.Name = model.Name;
            var result = await roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                TempData["Success"] = "Role updated successfully!";
                return RedirectToAction("RoleIndex");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                TempData["Error"] = "Role not found.";
                return RedirectToAction("RoleIndex");
            }

            var result = await roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                TempData["Success"] = "Role deleted successfully!";
            }
            else
            {
                TempData["Error"] = string.Join("; ", result.Errors.Select(e => e.Description));
            }

            return RedirectToAction("RoleIndex");
        }
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }




    }
}
