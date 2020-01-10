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
        private readonly IBug _bug;

        public AccountController(IBug bug,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUser user)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _bug = bug;
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
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Team = _bug.GetTeamByName(model.Team)
                };

                if (await _user.IsUserUniqueAsync(user))
                {
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
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
                var user = await _userManager.FindByNameAsync(roleName);
                if (user != null)
                {
                    await _signInManager.SignInAsync(user, false);
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> UserProfile()
        {
            var userName = User.Identity.Name;
            var currentUser = await _userManager.FindByNameAsync(userName);
            // generate new view model to send 
            var model = new UserProfileModel()
            {
                FullName = currentUser.FirstName + " " + currentUser.LastName,
                EmailAddress = currentUser.Email,
                Team = _user.GetTeamName(currentUser),
                UserName = userName,
                Role = await _user.GetAllRolesAsync(currentUser, ',')
            };
            return View(model);
        }

    }
}