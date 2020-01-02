using System.Linq;
using System.Threading.Tasks;
using BugTrackerV1._0.Models.IdentityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BugTrackerV1._0.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public IdentityController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user != null)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            //var role = "Admin";
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                var emailUnique = await _userManager.FindByNameAsync(user.UserName);
                var usernameUnique = await _userManager.FindByEmailAsync(user.Email);

                if(emailUnique == null && usernameUnique == null)
                {
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        //var currentUser = await _userManager.FindByEmailAsync(user.Email);

                        //// create role if it doesn't exists
                        //var roleResult = await _roleManager.FindByNameAsync(role);
                        //if (roleResult == null)
                        //{
                        //    var adminRole = new IdentityRole(role);
                        //    await _roleManager.CreateAsync(adminRole);
                        //    // await _roleManager.AddClaimAsync(adminRole, new Claim("Can add roles", "add.role"));
                        //}
                        //await _userManager.AddToRoleAsync(currentUser, role);

                        var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                        if (signInResult.Succeeded)
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ManageRoles()
        {
            var model = new AssignRolesIndexModel
            {
                Usernames = _userManager.Users.Select(user => user.UserName).ToList(),
                Roles = _roleManager.Roles.Select(role => role.Name).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRoles(AssignRolesIndexModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.User);
                await _userManager.AddToRoleAsync(user, model.Role);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRoles(string roleName)
        {
            if (ModelState.IsValid)
            {
                // create role if it doesn't exists
                var roleResult = await _roleManager.FindByNameAsync(roleName);
                if (roleResult == null)
                {
                    var newRole = new IdentityRole(roleName);
                    await _roleManager.CreateAsync(newRole);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoles(string roleName)
        {
            if (ModelState.IsValid)
            {
                var roleResult = await _roleManager.FindByNameAsync(roleName);
                if(roleResult != null)
                {
                    await _roleManager.DeleteAsync(roleResult);
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}