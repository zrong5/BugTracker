using System;
using System.Threading.Tasks;
using BugTracker.Models.SubmissionModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BugTracker.Data;
using BugTracker.Data.Models;
using Microsoft.AspNetCore.Authorization;

namespace BugTracker.Controllers
{
    public class SubmissionController : Controller
    {
        private readonly IBug _bug;
        private readonly IUserBug _userBug;
        private readonly UserManager<ApplicationUser> _userManager;
        public SubmissionController(IBug bug, 
            UserManager<ApplicationUser> userManager,
            IUserBug userBug)
        {
            _bug = bug;
            _userManager = userManager;
            _userBug = userBug;
        }

        [Authorize (Policy = "Submit Bugs")]
        public async Task<IActionResult> IndexAsync()
        {
            SubmissionOptionsModel options = null;
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            if (User.IsInRole("Admin"))
            {
                options = new SubmissionOptionsModel()
                {
                    ProjectOptions = _bug.GetAllProjects(),
                    TeamOptions = _bug.GetAllTeams(),
                    UrgencyOptions = _bug.GetAllUrgencies()
                };
            }
            else if (User.IsInRole("Manager"))
            {
                options = new SubmissionOptionsModel()
                {
                    ProjectOptions = _bug.GetAllProjects(),
                    TeamMemberOptions = await _userBug.GetAllTeamMembersAsync(user),
                    UrgencyOptions = _bug.GetAllUrgencies()
                };
            }
            
            var model = new SubmissionIndexModel()
            {
                Options = options   
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Submit Bugs")]
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
                    AssignedTo = model.TeamMember == null? null: await _userManager.FindByNameAsync(model.TeamMember),
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