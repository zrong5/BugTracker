using System.Linq;
using BugTracker.Models.BugModels;
using Microsoft.AspNetCore.Mvc;
using BugTracker.Data;
using BugTracker.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BugTracker.Controllers
{
    public class BugController : Controller
    {
        private readonly IBug _bugs;
        private readonly IUserBug _userBug;
        private readonly UserManager<ApplicationUser> _userManager;

        public BugController(IBug bugs,
            UserManager<ApplicationUser> userManager,
            IUserBug userBug)
        {
            _bugs = bugs;
            _userManager = userManager;
            _userBug = userBug;
        }

        [Authorize(Policy = "View Bugs")]
        public async Task<IActionResult> IndexAsync()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name).ConfigureAwait(false);
            var bugModels = await _userBug.GetAllBugsByUserAsync(currentUser).ConfigureAwait(false);
            var listingModel = bugModels
                .Select(result => new BugListingModel
                {
                    Id = result.Id,
                    Title = result.Title,
                    Urgency = result.Urgency?.Level,
                    Status = result.Status?.Name,
                    Team = result.Owner?.Name,
                    CreatedOn = result.CreatedOn,
                    AssignedTo = result.AssignedTo == null ? "" : 
                    result.AssignedTo.FirstName + " " 
                        + result.AssignedTo.LastName
                });
            var model = new BugIndexModel()
            {
                Bugs = listingModel
            };
            return View(model);
        }
        [Authorize(Policy = "View Bugs")]
        public async Task<IActionResult> DetailAsync(int id)
        {
            var bug = _bugs.GetById(id);
            var detail = bug.LogDetail == null ? "" : bug.LogDetail.Detail;
            var createdByFullName = bug.CreatedBy.FirstName + " " + bug.CreatedBy.LastName + " @";
            var closedByFullName = "";
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name).ConfigureAwait(false);
            if(bug.ClosedBy != null)
            {
                closedByFullName = bug.ClosedBy.FirstName + " " + bug.ClosedBy.LastName + " @";
            }
            var model = new BugDetailModel()
            {
                Id = id,
                Title = bug.Title,
                ProgressLog = detail,
                Project = bug.ProjectAffected?.Name,
                Status = bug.Status?.Name,
                Team = bug.Owner?.Name,
                Description = bug.Description,
                Urgency = bug.Urgency?.Level,
                CreatedOn = bug.CreatedOn,
                CreatedBy = createdByFullName + bug.CreatedBy.UserName,
                ClosedOn = bug.ClosedOn,
                ClosedBy = bug.ClosedBy == null ? null : closedByFullName + bug.ClosedBy.UserName,
                StatusOptions = _bugs.GetAllStatus().Select(status => status.Name),
                DeveloperOptions = (await _userBug.GetAllTeamMembersAsync(currentUser).ConfigureAwait(false))
                                    .Select(u => u.UserName),
                ProjectOptions = (await _userBug.GetAllProjectByUserAsync(currentUser).ConfigureAwait(false))
                                    .Select(proj => proj.Name),
                UserProjectOptions = _userBug.GetAllUserProjects().Select(result => new UserProjectModel
                {
                    Username = result.User.UserName,
                    Project = result.Project.Name
                }),

                UpdateDetail = new BugUpdateModel()
            };  
            return View(model);
        }
        [HttpPost]
        [Authorize(Policy = "View Bugs")]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateBug(BugDetailModel model, int Id)
        {
            if (ModelState.IsValid && model != null)
            {
                string trimmedMsg = "";
                if (model.UpdateDetail.UpdateToLog != null)
                {
                    trimmedMsg = model.UpdateDetail.UpdateToLog.Trim();
                }
                _bugs.Update(Id, trimmedMsg, model.UpdateDetail.NewStatus, model.UpdateDetail.AssignedTo, model.UpdateDetail.Project);
            }
            return LocalRedirect("/Bug/Detail/" + Id);
        }
    }
}