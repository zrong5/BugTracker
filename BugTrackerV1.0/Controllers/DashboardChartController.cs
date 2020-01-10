using BugTracker.Data;
using BugTracker.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BugTracker.Models.BugModels
{
    public class DashboardChartController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IChart _chart;
        public DashboardChartController(
            UserManager<ApplicationUser> userManager,
            IChart chart)
        {
            _userManager = userManager;
            _chart = chart;
        }

        [Authorize(Policy = "View Dashboard")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy = "View Dashboard")]
        public async Task<JsonResult> PopulateStatusGraphAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var statusList = await _chart.GetBugByStatusListAsync(user);
            return Json(statusList);
        }
        [HttpGet]
        [Authorize(Policy = "View Dashboard")]
        public async Task<JsonResult> PopulateMonthlyGraphAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var monthList = await _chart.GetBugByMonthListAsync(user);
            return Json(monthList);
        }
        [HttpGet]
        [Authorize(Policy = "View Dashboard")]
        public async Task<JsonResult> PopulateUrgencyGraphAsync()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var urgencyList = await _chart.GetBugByUrgencyListAsync(user);
            return Json(urgencyList);
        }
    }
}