using System;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Data.Models;
using BugTracker.Models.ManagementModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BugTracker.Controllers
{
    public class ManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IUser _user;
        private readonly IBug _bug;
        public ManagementController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IUser user, IBug bug)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _user = user;
            _bug = bug;
        }


        [Authorize(Policy = "Manage Roles")]
        public IActionResult ManageRoles()
        {
            var model = new RoleIndexModel
            {
                Usernames = _userManager.Users.Select(user => user.UserName).ToList(),
                Roles = _roleManager.Roles.Select(role => role.Name).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Manage Roles")]
        public async Task<IActionResult> ManageRolesAssignment(RoleIndexModel model, string option)
        {
            if (ModelState.IsValid)
            {
                if (option == "Assign")
                {
                    return await AssignUserToRole(model);
                }
                else
                {
                    return await RemoveUserFromRole(model);
                }
            }
            return RedirectToAction("ManageRoles", "Management");
        }

        [Authorize(Policy = "Manage Roles")]
        public async Task<IActionResult> AssignUserToRole(RoleIndexModel model)
        {

            var user = await _userManager.FindByNameAsync(model.UpdateModel.User);
            await _userManager.AddToRoleAsync(user, model.UpdateModel.Role);
            return RedirectToAction("ManageRoles", "Management");
        }

        [Authorize(Policy = "Manage Roles")]
        public async Task<IActionResult> RemoveUserFromRole(RoleIndexModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UpdateModel.User);
            await _userManager.RemoveFromRoleAsync(user, model.UpdateModel.Role);
            return RedirectToAction("ManageRoles", "Management");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Manage Roles")]
        public async Task<IActionResult> AddRoles(string roleName)
        {
            if (ModelState.IsValid)
            {
                // create role if it doesn't exists
                var roleResult = await _roleManager.FindByNameAsync(roleName);
                if (roleResult == null)
                {
                    var newRole = new IdentityRole<Guid>(roleName);
                    await _roleManager.CreateAsync(newRole);
                }
            }
            return RedirectToAction("ManageRoles", "Management");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Manage Roles")]
        public async Task<IActionResult> DeleteRoles(string roleName)
        {
            if (ModelState.IsValid)
            {
                var roleResult = await _roleManager.FindByNameAsync(roleName);
                if (roleResult != null)
                {
                    await _roleManager.DeleteAsync(roleResult);
                }
            }
            return RedirectToAction("ManageRoles", "Management");
        }


        [Authorize(Policy = "Manage Projects")]
        public IActionResult ManageProjects()
        {
            var allProjects = _bug.GetAllProjects();
            var allUsers = _user.GetAll();
            var userProjects = _user.GetAllUserProjects();

            // list anyone that is not just a submitter
            var listingModel = userProjects
                .Where(userProj => !((_userManager.IsInRoleAsync(userProj.User, "Submitter")).Result))
                .Select(result => new ProjectListingModel
                {
                    Username = result.User.UserName,
                    EmailAddress = result.User.Email,
                    Project = result.Project.Name
                });

            // list anyone that is not just a submitter
            var model = new ProjectIndexModel
            {
                UserProjects = listingModel,
                Projects = _bug.GetAllProjects().Select(result => result.Name),
                Users = _user.GetAll()
                    .Where(user => !((_userManager.IsInRoleAsync(user, "Submitter")).Result))
                    .Select(result => result.UserName)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Manage Projects")]
        public IActionResult ManageProjects(ProjectIndexModel model, string option)
        {
            // redirect to differect actions depending on which button is pressed
            if (ModelState.IsValid)
            {
                if(option == "Assign")
                {
                    return AssignUserToProject(model);
                }
                else
                {
                    return RemoveUserFromProject(model);
                }
            }
            return RedirectToAction("ManageProjects", "Management");
        }

        [Authorize(Policy = "Manage Projects")]
        public IActionResult AssignUserToProject(ProjectIndexModel model)
        {
            var user = (_userManager.FindByNameAsync(model.UpdateModel.Username)).Result;
            _user.AssignUserToProject(user, model.UpdateModel.ProjectName);
            return RedirectToAction("ManageProjects", "Management");
        }

        [Authorize(Policy = "Manage Projects")]
        public IActionResult RemoveUserFromProject(ProjectIndexModel model)
        {
            var user = (_userManager.FindByNameAsync(model.UpdateModel.Username)).Result;
            _user.RemoveUserFromProject(user, model.UpdateModel.ProjectName);
            return RedirectToAction("ManageProjects", "Management");
        }
    }
}