using System;
using System.Threading.Tasks;
using BugTracker.Models.SubmissionModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BugTracker.Data;
using BugTracker.Data.Models;

namespace BugTracker.Controllers
{
    public class SubmissionController : Controller
    {
        private readonly IBug _bug;
        private readonly UserManager<ApplicationUser> _userManager;
        public SubmissionController(IBug bug, UserManager<ApplicationUser> userManager)
        {
            _bug = bug;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var options = new SubmissionOptionsModel()
            {
                ProjectOptions = _bug.GetAllProjects(),
                TeamOptions = _bug.GetAllTeams(),
                UrgencyOptions = _bug.GetAllUrgencies()
            };
            var model = new SubmissionIndexModel()
            {
                Options = options   
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(SubmissionIndexModel model)
        {
            int redirectId = 0;
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var now = DateTime.Now;
                var newBug = new Bug()
                {
                    Urgency = _bug.GetUrgencyByName(model.Urgency),
                    Title = model.Title,
                    Owner = _bug.GetTeamByName(model.Team),
                    CreatedOn = now,
                    Description = model.Description,
                    LogDetail = _bug.CreateEmptyLog(),
                    ProjectAffected = _bug.GetProjectByName(model.ProjectAffected),
                    Status = _bug.GetStatusByName("Open"),
                    CreatedBy = currentUser
                };
                redirectId = _bug.Add(newBug);
            }
            return LocalRedirect("/Bug/Detail/" + redirectId.ToString());
        }
    }
}