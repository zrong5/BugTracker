using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Data.Models;
using BugTracker.Models.AccountModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUser _user;

        public AccountController(IUser user,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _user = user;
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
            if (ModelState.IsValid && model != null)
            {
                var user = await _userManager.FindByNameAsync(model.Username)
                    .ConfigureAwait(false);
                if (user != null)
                {
                    var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false)
                            .ConfigureAwait(false);
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
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterIndexModel model)
        {
            if (ModelState.IsValid && model != null)
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = await _user.CreateUniqueUsernameAsync(model.FirstName, model.LastName)
                        .ConfigureAwait(false),
                    Email = model.Email,
                };

                if (await _user.IsUserUniqueAsync(user).ConfigureAwait(false))
                {
                    var result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);
                    if (result.Succeeded)
                    {
                        var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false)
                            .ConfigureAwait(false);
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
            await _signInManager.SignOutAsync().ConfigureAwait(false);
            return RedirectToAction("Login", "Account");
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
                var user = await _userManager.FindByNameAsync(roleName).ConfigureAwait(false);
                if (user != null)
                {
                    await _signInManager.SignInAsync(user, false).ConfigureAwait(false);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UserProfile()
        {
            var userName = User.Identity.Name;
            var currentUser = await _userManager.FindByNameAsync(userName).ConfigureAwait(false);
            // generate new view model to send 
            var model = new UserProfileModel()
            {
                FullName = currentUser.FirstName + " " + currentUser.LastName,
                EmailAddress = currentUser.Email,
                Team = _user.GetTeamName(currentUser),
                UserName = userName,
                Role = await _user.GetAllRolesAsync(currentUser, ',').ConfigureAwait(false)
            };
            return View(model);
        }

    }
}