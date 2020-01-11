using BugTracker.Data;
using BugTracker.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
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
        public bool AssignUserToProject(ApplicationUser user, string projectName)
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

            if (project != null && !alreadyAssigned)
            {
                _context.Add(userProject);
                _context.SaveChanges();
                return true;
            }
            return false;
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
                try
                {
                    var team = _userManager.Users
                    .Include(user => user.Team)
                    .FirstOrDefault(u => u.Id == user.Id)
                    .Team;
                    return GetAllBugs().Where(bug => bug.Owner == team);
                }
                catch (NullReferenceException)
                {

                }
                
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

        public bool RemoveUserFromProject(ApplicationUser user, string projectName)
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
                return true;
            }
            return false;
        }

        public bool AssignUserToTeam(ApplicationUser user, string teamName)
        {
            var newTeam = _context.Team.FirstOrDefault(team => team.Name == teamName);
            var alreadyAssigned = _context.Users
                .Where(u => u == user && user.Team == newTeam)
                .Any();

            if (newTeam != null && !alreadyAssigned)
            {
                _context.Update(user);
                user.Team = newTeam;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void AddTeam(Team newTeam)
        {
            _context.Add(newTeam);
            _context.SaveChanges();
        }

        public void DeleteTeam(Team toDelete)
        {
            _context.Remove(toDelete);
            _context.SaveChanges();
        }
    }
}
