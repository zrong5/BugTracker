using System;
using System.Linq;
using System.Threading.Tasks;
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
        public ManagementController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
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
    }
}