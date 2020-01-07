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
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("UserProfile", "Identity");
            }
            else
            {
                return RedirectToAction("Login", "Identity");
            }
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
