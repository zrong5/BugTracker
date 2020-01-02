using System.Security.Claims;
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
        public async Task<IActionResult> Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user != null)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
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
            var username = model.Username;
            var password = model.Password;
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = username,
                    Email = "userEmail@asp.net"
                };
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    var currentUser = await _userManager.FindByEmailAsync(user.Email);

                    // create role if it doesn't exists
                    // var roleResult = await _roleManager.FindByNameAsync(role);
                    // if (roleResult == null)
                    // {
                        // var adminRole = new IdentityRole(role);
                        // await _roleManager.CreateAsync(adminRole);
                        //await _roleManager.AddClaimAsync(adminRole, new Claim("Can add roles", "add.role"));
                    // }
                    // await _userManager.AddToRoleAsync(currentUser, role);

                    var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                    if (signInResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
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
    }
}