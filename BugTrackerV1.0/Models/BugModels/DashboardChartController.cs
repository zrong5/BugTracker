using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<JsonResult> PopulateStatusGraph()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var statusList = _chart.GetBugByStatusList(user);
            return Json(statusList);
        }
        [HttpGet]
        public async Task<JsonResult> PopulateMonthlyGraph()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var monthList = _chart.GetBugByMonthList(user);
            return Json(monthList);
        }
        [HttpGet]
        public async Task<JsonResult> PopulateUrgencyGraph()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var urgencyList = _chart.GetBugByUrgencyList(user);
            return Json(urgencyList);
        }
    }
}