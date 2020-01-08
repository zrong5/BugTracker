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
        [Authorize(Policy = "View Dashboard")]
        public JsonResult PopulateStatusGraph()
        {
            var user = (_userManager.FindByNameAsync(User.Identity.Name)).Result;
            var statusList = _chart.GetBugByStatusList(user);
            return Json(statusList);
        }
        [HttpGet]
        [Authorize(Policy = "View Dashboard")]
        public JsonResult PopulateMonthlyGraph()
        {
            var user = (_userManager.FindByNameAsync(User.Identity.Name)).Result;
            var monthList = _chart.GetBugByMonthList(user);
            return Json(monthList);
        }
        [HttpGet]
        [Authorize(Policy = "View Dashboard")]
        public JsonResult PopulateUrgencyGraph()
        {
            var user = (_userManager.FindByNameAsync(User.Identity.Name)).Result;
            var urgencyList = _chart.GetBugByUrgencyList(user);
            return Json(urgencyList);
        }
    }
}