﻿using System.Linq;
using BugTracker.Models.BugModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BugTracker.Data;
using BugTracker.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BugTracker.Controllers
{
    public class BugController : Controller
    {
        private readonly IBug _bugs;
        private readonly UserManager<ApplicationUser> _userManager;
        public BugController(IBug bugs,
            UserManager<ApplicationUser> userManager)
        {
            _bugs = bugs;
            _userManager = userManager;
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
                ClosedBy = bug.ClosedBy == null ? "" : bug.ClosedBy.UserName,
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
                string trimmedMsg = "";
                if (model.UpdateDetail.UpdateToLog != null)
                {
                    trimmedMsg = model.UpdateDetail.UpdateToLog.Trim();
                }
                _bugs.Update(Id, trimmedMsg, model.UpdateDetail.NewStatus);
            }
            return LocalRedirect("/Bug/Detail/" + Id);
        }

        [Authorize(Policy = "View Dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> PopulateStatusGraph()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var statusList = _bugs.GetBugByStatusList(user);
            return Json(statusList);
        }
        [HttpGet]
        public async Task<JsonResult> PopulateMonthlyGraph()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var monthList = _bugs.GetBugByMonthList(user);
            return Json(monthList);
        }
        [HttpGet]
        public async Task<JsonResult> PopulateUrgencyGraph()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var urgencyList = _bugs.GetBugByUrgencyList(user);
            return Json(urgencyList);
        }
    }
}