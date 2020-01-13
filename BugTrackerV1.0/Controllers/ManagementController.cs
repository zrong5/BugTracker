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
        private readonly IUserBug _userBug;
        public ManagementController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IUser user, IBug bug, IUserBug userBug)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _user = user;
            _bug = bug;
            _userBug = userBug;
        }


        [Authorize(Policy = "Manage Roles")]
        public IActionResult ManageRoles()
        {
            var listingModel = _user.GetAll()
                .Select(result => new RoleListingModel
                {
                    FullName = result.FirstName + " " + result.LastName,
                    Username = result.UserName,
                    EmailAddress = result.Email,
                    Roles = _user.GetAllRolesAsync(result, ',').Result
                });
            var model = new RoleIndexModel
            {
                UserRoles = listingModel,
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
            if(!string.IsNullOrEmpty(model.UpdateModel.User) 
                && !string.IsNullOrEmpty(model.UpdateModel.Role))
            {
                var user = await _userManager.FindByNameAsync(model.UpdateModel.User);
                await _userManager.AddToRoleAsync(user, model.UpdateModel.Role);
            }
            return RedirectToAction("ManageRoles", "Management");
        }

        [Authorize(Policy = "Manage Roles")]
        public async Task<IActionResult> RemoveUserFromRole(RoleIndexModel model)
        {
            if (!string.IsNullOrEmpty(model.UpdateModel.User)
                && !string.IsNullOrEmpty(model.UpdateModel.Role))
            {
                var user = await _userManager.FindByNameAsync(model.UpdateModel.User);
                await _userManager.RemoveFromRoleAsync(user, model.UpdateModel.Role);
            }
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
        public async Task<IActionResult> ManageProjectsAsync()
        {
            var allProjects = _bug.GetAllProjects();
            var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
            var allUsers = await _userBug.GetAllTeamMembersAsync(currentUser);
            var userProjects = await _userBug.GetGetAllUserProjectsByUserAsync(currentUser);
            var allTeams = await _userBug.GetAllTeamsByUser(currentUser);
            var assignedListingModel = userProjects
                .Select(result => new AssignedProjectListingModel
                {
                    FullName = result.User.FirstName + " " + result.User.LastName,
                    Username = result.User.UserName,
                    EmailAddress = result.User.Email,
                    Project = result.Project.Name
                });
            var listingModel = allProjects
                .Select(result => new ProjectListingModel
                {
                    Project = result.Name,
                    Manager = (_userBug.GetManagerAsync(result.Owner).Result)?.UserName,
                    Team = result.Owner?.Name,
                    Email = (_userBug.GetManagerAsync(result.Owner).Result)?.Email
                });
            // list anyone that is not just a submitter
            var model = new ProjectIndexModel
            {
                UserProjects = assignedListingModel,
                TeamProjects = listingModel,
                Projects = (await _userBug.GetAllProjectByUserAsync(currentUser)).Select(proj => proj.Name),
                Users = allUsers.Select(user => user.UserName),
                Teams = allTeams.Select(team => team.Name)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Manage Projects")]
        public async Task<IActionResult> ManageProjectsAsync(ProjectIndexModel model, string option)
        {
            // redirect to differect actions depending on which button is pressed
            if (ModelState.IsValid)
            {
                if(option == "Assign")
                {
                    return await AssignEntityToProjectAsync(model);
                }
                else
                {
                    return await RemoveEntityFromProjectAsync(model);
                }
            }
            return RedirectToAction("ManageProjects", "Management");
        }

        [Authorize(Policy = "Manage Projects")]
        public async Task<IActionResult> AssignEntityToProjectAsync(ProjectIndexModel model)
        {
            var project = _bug.GetProjectByName(model.UpdateModel.ProjectName);
            if (User.IsInRole("Manager"))
            {
                var user = await _userManager.FindByNameAsync(model.UpdateModel.Username);
                _userBug.AssignUserToProject(user, project);
            }
            else
            {
                var team = _bug.GetTeamByName(model.UpdateModel.Team);
                _userBug.AssignTeamToProject(team, project);
            }
            return RedirectToAction("ManageProjects", "Management");
        }

        [Authorize(Policy = "Manage Projects")]
        public async Task<IActionResult> RemoveEntityFromProjectAsync(ProjectIndexModel model)
        {
            var project = _bug.GetProjectByName(model.UpdateModel.ProjectName);
            if (User.IsInRole("Manager"))
            {
                var user = await _userManager.FindByNameAsync(model.UpdateModel.Username);
                _userBug.RemoveUserFromProject(user, project);
            }
            else
            {
                var team = _bug.GetTeamByName(model.UpdateModel.Team);
                _userBug.RemoveTeamFromProject(team, project);
            }
            return RedirectToAction("ManageProjects", "Management");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Manage Projects")]
        public async Task<IActionResult> AddProjectAsync(ProjectIndexModel model)
        {
            if (ModelState.IsValid)
            {
                var createModel = model.CreateModel;
                if (User.IsInRole("Admin"))
                {
                    var newProject = new Project
                    {
                        Name = createModel.ProjectName,
                        Description = createModel.Description
                    };
                    _userBug.AddProject(newProject);
                }
                else
                {
                    var assignTo = await _userManager.FindByNameAsync(createModel.Developer);
                    var newProject = new Project
                    {
                        Name = createModel.ProjectName,
                        Description = createModel.Description,
                        Owner = (await _userBug.GetAllTeamsByUser(assignTo)).FirstOrDefault()
                    };
                    
                    _userBug.AssignUserToProject(assignTo, newProject);
                }
            }
            return RedirectToAction("ManageProjects", "Management");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Manage Projects")]
        public IActionResult DeleteProject(string projectName)
        {
            if (ModelState.IsValid)
            {
                var project = _bug.GetProjectByName(projectName);
                _userBug.DeleteProject(project);
            }
            return RedirectToAction("ManageProjects", "Management");
        }

        [Authorize(Policy = "Admin Permission")]
        public IActionResult ManageTeams()
        {
            var listingModel = _user.GetAll()
                .Select(result => new TeamListingModel
                {
                    FullName = result.FirstName + " " + result.LastName,
                    Username = result.UserName,
                    Email = result.Email,
                    Team = _user.GetTeamName(result),
                    Role = _user.GetAllRolesAsync(result, ',').Result
                });
            var model = new TeamIndexModel
            {
                UserTeams = listingModel,
                Usernames = _userManager.Users.Select(user => user.UserName).ToList(),
                Teams = _bug.GetAllTeams().Select(team => team.Name).ToList(),
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin Permission")]
        public async Task<IActionResult> AssignTeamsAsync(TeamIndexModel model)
        {
            if (ModelState.IsValid)
            {
                var updateModel = model.UpdateModel;
                if (!string.IsNullOrEmpty(updateModel.Username)
                    && !string.IsNullOrEmpty(updateModel.Team))
                {
                    var user = await _userManager.FindByNameAsync(updateModel.Username);
                    var succeeded = _userBug.AssignUserToTeam(user, updateModel.Team);
                    if (succeeded)
                    {
                        return RedirectToAction("ManageTeams", "Management");
                    }
                }
            }
            return RedirectToAction("ManageTeams", "Management");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin Permission")]
        public IActionResult AddTeams(string teamName)
        {
            if (ModelState.IsValid)
            {
                var team = _bug.GetTeamByName(teamName);
                if (team == null) 
                {
                    var newTeam = new Team
                    {
                        Name = teamName
                    };
                    _userBug.AddTeam(newTeam);
                    return RedirectToAction("ManageTeams", "Management");
                }
            }
            return RedirectToAction("ManageTeams", "Management");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Admin Permission")]
        public IActionResult DeleteTeams(string teamName)
        {
            if (ModelState.IsValid)
            {
                var team = _bug.GetTeamByName(teamName);
                if(team != null)
                {
                    _userBug.DeleteTeam(team);
                    return RedirectToAction("ManageTeams", "Management");
                }
            }
            return RedirectToAction("ManageTeams", "Management");
        }
    }
}