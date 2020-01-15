using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Data.Models;
using BugTracker.Models.ProjectModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class ProjectController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IBug _bug;
        private readonly IUserBug _userBug;

        public ProjectController(
            UserManager<ApplicationUser> userManager,
            IBug bug, IUserBug userBug)
        {
            _userManager = userManager;
            _bug = bug;
            _userBug = userBug;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name)
                .ConfigureAwait(false);
            var projectsByUser = await _userBug.GetAllProjectByUserAsync(currentUser)
                .ConfigureAwait(false);

            var listingModel = projectsByUser.Select(proj => new ProjectListingModel 
            { 
                Name = proj.Name,
                Team = proj.Owner.Name,
                AssignedTo = _userBug.GetUserByProject(proj).UserName,
                Id = proj.Id
            });

            var model = new ProjectIndexModel
            {
                ListingModel = listingModel
            };
            return View(model);
        }

        public IActionResult Detail(Guid id)
        {
            var project = _bug.GetProjectById(id);
            var model = new ProjectDetailModel
            {
                AssignedTo = _userBug.GetUserByProject(project).UserName,
                Description = project.Description,
                Email = _userBug.GetUserByProject(project).Email,
                Name = project.Name,
                Team = project.Owner.Name
            };

            return View(model);
        }
    }
}