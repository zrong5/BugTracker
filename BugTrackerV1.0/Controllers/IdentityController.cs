using System;
using System.Linq;
using System.Threading.Tasks;
using BugTrackerV1._0.Models.IdentityModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TrackerData;
using TrackerData.Models;

namespace BugTrackerV1._0.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IBug _bug;
        public IdentityController(
            IBug bug,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _bug = bug;
        }
        [AllowAnonymous]
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
        [AllowAnonymous]
        public IActionResult Register()
        {
            var optionsModel = new RegisterOptionsModel
            {
                TeamOptions = _bug.GetAllTeams()
            };
            var model = new RegisterIndexModel
            {
                Options = optionsModel
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterIndexModel model)
        {
            //var role = "Admin";
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Team = _bug.GetTeamByName(model.Team)
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
            return RedirectToAction("Login", "Identity");
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
                    var newRole = new IdentityRole<Guid>(roleName);
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
        [AllowAnonymous]
        public IActionResult RoleSelect()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> RoleSelect(string roleName)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(roleName);
                if(user != null)
                {
                    await _signInManager.SignInAsync(user, false);
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}