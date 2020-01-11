using BugTracker.Data;
using BugTracker.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Service
{
    public class UserBugService : IUserBug
    {
        private readonly TrackerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserBugService(TrackerContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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

        protected IEnumerable<Bug> GetAllBugs()
        {
            return _context.Bug
                    .Include(bug => bug.Status)
                    .Include(bug => bug.ProjectAffected)
                    .Include(bug => bug.Owner)
                    .Include(bug => bug.LogDetail)
                    .Include(bug => bug.Urgency)
                    .Include(bug => bug.CreatedBy)
                    .Include(bug => bug.ClosedBy)
                    .Include(bug => bug.AssignedTo);
        }

        public async Task<IEnumerable<Bug>> GetAllBugsByUserAsync(ApplicationUser user)
        {
            if(await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return GetAllBugs();
            }
            else if (await _userManager.IsInRoleAsync(user, "Manager"))
            {
                var team = user.Team;
                return GetAllBugs().Where(bug => bug.Owner == team);
            }
            return GetAllBugs().Where(bug => bug.AssignedTo == user);
        }

        public IEnumerable<ApplicationUser> GetAllTeamMembers(ApplicationUser manager)
        {
            return _userManager.Users
                .Include(user => user.Team)
                .Where(user => user.Team == manager.Team);
        }

        public IEnumerable<UserProject> GetAllUserProjects()
        {
            return _context.UserProject;
        }

        public void RemoveUserFromProject(ApplicationUser user, string projectName)
        {
            var projectsOfUser = _context.UserProject
                .Where(userProject => userProject.User == user);
            var project = _context.Project
                .FirstOrDefault(project => project.Name == projectName);

            var toDelete = projectsOfUser
                .FirstOrDefault(userProject => userProject.Project.Name == projectName);

            if (toDelete != null)
            {
                _context.Remove(toDelete);
                _context.SaveChanges();
            }
        }
    }
}
