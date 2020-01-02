using System;
using BugTrackerV1._0.Models.SubmissionModels;
using Microsoft.AspNetCore.Mvc;
using TrackerData;

namespace BugTrackerV1._0.Controllers
{
    public class SubmissionController : Controller
    {
        private IBug _bug;
        public SubmissionController(IBug bug)
        {
            _bug = bug;
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
        public IActionResult Submit(SubmissionIndexModel model)
        {
            int redirectId = 0;
            if (ModelState.IsValid)
            {
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
                    Status = _bug.GetStatusByName("Open")
                };
                redirectId = _bug.Add(newBug);
            }
            return LocalRedirect("/Bug/Detail/" + redirectId.ToString());
        }
    }
}