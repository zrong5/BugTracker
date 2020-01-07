using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BugTrackerV1._0.Models;
using BugTrackerV1._0.Models.IdentityModels;
using Microsoft.AspNetCore.Identity;
using TrackerData.Models;
using System.Threading.Tasks;
using TrackerData;

namespace BugTrackerV1._0.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUser _user;
        private readonly UserManager<ApplicationUser> _userManager;
        public HomeController(ILogger<HomeController> logger, IUser user, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _user = user;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("UserProfile", "Home");
            }
            else
            {
                return RedirectToAction("Login", "Identity");
            }
        }
        public async Task<IActionResult> UserProfile()
        {
            var userName = User.Identity.Name;
            var currentUser = await _userManager.FindByNameAsync(userName);

            // generate new view model to send 
            var model = new UserProfileModel()
            {
                EmailAddress = currentUser.Email,
                Team = _user.GetTeamName(currentUser),
                UserName = userName,
                Role = await _user.GetAllRoles(currentUser, ',')
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
