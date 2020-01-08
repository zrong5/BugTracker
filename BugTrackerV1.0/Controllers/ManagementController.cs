using System;
using System.Collections.Generic;
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
        public ManagementController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IUser user)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _user = user;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult ManageRoles()
        {
            var model = new AssignRolesIndexModel
            {
                Usernames = _userManager.Users.Select(user => user.UserName).ToList(),
                Roles = _roleManager.Roles.Select(role => role.Name).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRoles(AssignRolesIndexModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.User);
                await _userManager.AddToRoleAsync(user, model.Role);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Policy = "Manage Projects")]
        public IActionResult ManageProjects()
        {
            var allProjects = _user.GetAllProjects();
            var allUsers = _user.GetAll();
            var listingModel = allUsers
                .Where(user => (_userManager.IsInRoleAsync(user, "Admin")).Result 
                    || (_userManager.IsInRoleAsync(user, "Manager")).Result)
                .Select(result => new ProjectListingModel
            {
                Username = result.UserName,
                EmailAddress = result.Email,
                Project = result.Project.Name
            });
            var model = new AssignProjectsIndexModel
            {
                UserProjects = listingModel,
                Projects = _user.GetAllProjects().Select(result=>result.Name),
                Users = _user.GetAll().Select(result => result.UserName)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Manage Projects")]
        public IActionResult ManageProjects(AssignProjectsIndexModel model)
        {
            if (ModelState.IsValid)
            {
                var user = (_userManager.FindByNameAsync(model.UpdateModel.Username)).Result;
                _user.AssignUserToProject(user, model.UpdateModel.ProjectName);
            }
            return RedirectToAction("ManageProjects", "Management");
        }
    }
}