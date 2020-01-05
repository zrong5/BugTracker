using System.Linq;
using BugTrackerV1._0.Models.BugModels;
using Microsoft.AspNetCore.Mvc;
using TrackerData;

namespace BugTrackerV1._0.Controllers
{
    public class BugController : Controller
    {
        private IBug _bugs;
        public BugController(IBug bugs)
        {
            _bugs = bugs;
        }
        public IActionResult Index()
        {
            var bugModels = _bugs.GetAll();
            var listingModel = bugModels
                .Select(result => new BugListingModel
                {
                    Id = result.Id,
                    Title = result.Title,
                    Urgency = result.Urgency.Level,
                    Status = result.Status.Name,
                    Team = result.Owner.Name,
                    CreatedOn = result.CreatedOn
                });
            var model = new BugIndexModel()
            {
                Bugs = listingModel
            };
            return View(model);
        }
        public IActionResult Detail(int id)
        {
            var bug = _bugs.GetById(id);
            var detail = bug.LogDetail == null ? "" : bug.LogDetail.Detail;
            var model = new BugDetailModel()
            {
                Id = id,
                Title = bug.Title,
                ProgressLog = detail,
                Project = bug.ProjectAffected.Name,
                Status = bug.Status.Name,
                Team = bug.Owner.Name,
                Description = bug.Description,
                Urgency = bug.Urgency.Level,
                CreatedOn = bug.CreatedOn,
                CreatedBy = bug.CreatedBy.UserName,
                ClosedOn = bug.ClosedOn,
                ClosedBy = bug.ClosedBy.UserName,
                StatusOptions = _bugs.GetAllStatus(),
                UpdateDetail = new BugUpdateModel()
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateBug(BugDetailModel model, int Id)
        {
            if (ModelState.IsValid)
            {
                string trimmedMsg = model.UpdateDetail.UpdateToLog.Trim();
                _bugs.Update(Id, trimmedMsg, model.UpdateDetail.NewStatus);
            }
            return LocalRedirect("/Bug/Detail/" + Id);
        }
    }
}