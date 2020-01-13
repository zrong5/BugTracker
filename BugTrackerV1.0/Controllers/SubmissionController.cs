using System;
using System.Threading.Tasks;
using BugTracker.Models.SubmissionModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BugTracker.Data;
using BugTracker.Data.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

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
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name)
                .ConfigureAwait(false);

            if (User.IsInRole("Admin"))
            {
                options = new SubmissionOptionsModel()
                {
                    ProjectOptions = _bug.GetAllProjects().Select(proj => proj.Name),
                    TeamOptions = _bug.GetAllTeams().Select(team => team.Name),
                    UrgencyOptions = _bug.GetAllUrgencies().Select(urgency => urgency.Level)
                };
            }
            else if (User.IsInRole("Manager"))
            {
                options = new SubmissionOptionsModel()
                {
                    ProjectOptions = (await _userBug.GetAllProjectByUserAsync(currentUser)
                                        .ConfigureAwait(false))
                                        .Select(proj => proj.Name),
                    UserProjectOptions = _userBug.GetAllUserProjectsByUser(currentUser)
                                        .Select(result => new UserProjectModel
                                         {
                                            Username = result.User.UserName,
                                            Project = result.Project.Name
                                         }),
                    UrgencyOptions = _bug.GetAllUrgencies().Select(urgency => urgency.Level)

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
            if (ModelState.IsValid && model != null)
            {
                var currentUser = await _userManager.GetUserAsync(User).ConfigureAwait(false);
                var now = DateTime.Now;
                var newBug = new Bug()
                {
                    Urgency = _bug.GetUrgencyByName(model.Urgency),
                    Title = model.Title,
                    Owner = _bug.GetTeamByName(model.Team),
                    AssignedTo = model.Developer == null ? null : 
                            await _userManager.FindByNameAsync(model.Developer).ConfigureAwait(false),
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