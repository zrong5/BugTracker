using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Data.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;

namespace BugTracker.Service
{
    public class UserService : IUser
    {
        private readonly TrackerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(TrackerContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public void AssignUserToProject(ApplicationUser user, string projectName)
        {
            var project = _context.Project
                .FirstOrDefault(project => project.Name == projectName);
            var userProject = new UserProject
            {
                User = user,
                Project = project
            };

            var alreadyAssigned = _context.UserProject
                .Where(userProj => userProj.User == user && userProj.Project == project)
                .Any();

            if (!alreadyAssigned)
            {
                _context.Add(userProject);
                _context.SaveChanges();
            }
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.Users
                .Include(user => user.Team);
        }

        public async Task<string> GetAllRolesAsync(ApplicationUser user, char deliminator)
        {
            // concatenate all role names assign to user 
            var currentRoles = await _userManager.GetRolesAsync(user);
            var concatRoles = "";
            foreach (var role in currentRoles)
            {
                concatRoles += role + ", ";
            }
            // remove empty spaces and erase last ',' character
            concatRoles = concatRoles.Trim();
            concatRoles = concatRoles[0..^1];
            return concatRoles;
        }

        public IEnumerable<ApplicationUser> GetAllTeamMembers(ApplicationUser manager)
        {
            var team = manager.Team;
            return GetAll().Where(user => user.Team == team);
        }

        public IEnumerable<UserProject> GetAllUserProjects()
        {
            return _context.UserProject;
        }

        public string GetTeamName(ApplicationUser user)
        {
            return user.Team.Name;
        }

        public async Task<bool> IsUserUniqueAsync(ApplicationUser user)
        {
            var emailUnique = await _userManager.FindByNameAsync(user.UserName);
            var usernameUnique = await _userManager.FindByEmailAsync(user.Email);
            if(emailUnique == null && usernameUnique == null)
            {
                return true;
            }
            return false;
        }

        public void RemoveUserFromProject(ApplicationUser user, string projectName)
        {
            var projectsOfUser = _context.UserProject
                .Where(userProject => userProject.User == user);
            var project = _context.Project
                .FirstOrDefault(project => project.Name == projectName);

            var toDelete = projectsOfUser
                .FirstOrDefault(userProject => userProject.Project.Name == projectName);

            if(toDelete != null)
            {
                _context.Remove(toDelete);
                _context.SaveChanges();
            }
        }
    }
}
