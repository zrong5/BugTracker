using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Data.Models;
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


            return View();
        }
        public async Task<IActionResult> DetailAsync(Guid id)
        {
            

            var project = _bug.GetProjectById(id);


            return View();
        }
    }
}