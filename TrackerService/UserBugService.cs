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
        public bool AssignUserToProject(ApplicationUser user, Project project)
        {
            if (project != null && user != null)
            {
                var alreadyAssigned = _context.UserProject
                    .Include(up => up.Project)
                    .Include(up => up.User)
                    .Where(userProj => userProj.User == user && userProj.Project == project)
                    .Any();

                if (!alreadyAssigned)
                {
                    var userProject = new UserProject
                    {
                        User = user,
                        Project = project
                    };
                    _context.Add(userProject);
                    _context.SaveChanges();
                    return true;
                }
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

        protected Team GetTeam(ApplicationUser user)
        {
            return _userManager.Users
                    .Include(user => user.Team)
                    .FirstOrDefault(u => u.Id == user.Id)
                    .Team;
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
                    var team = GetTeam(user);
                    return GetAllBugs().Where(bug => bug.Owner == team);
                }
                catch (NullReferenceException)
                {

                }
                
            }
            return GetAllBugs().Where(bug => bug.AssignedTo == user);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllTeamMembersAsync(ApplicationUser user)
        {
            if(await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return await _userManager.GetUsersInRoleAsync("Manager");
            }
            var team = GetTeam(user);
            return _userManager.Users
                .Include(user => user.Team)
                .Where(u => u.Team == team);
        }

         public IEnumerable<UserProject> GetAllUserProjects()
        {
            return _context.UserProject
                .Include(up => up.Project)
                .Include(up => up.User);
        }

        public IEnumerable<UserProject> GetAllUserProjectsByUser(ApplicationUser user)
        {
            var team = GetTeam(user);
            return GetAllUserProjects().Where(up => up.Project.Owner == team);
        }

        public async Task<IEnumerable<Project>> GetAllProjectByUserAsync(ApplicationUser user)
        {
            if(await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return _context.Project.Include(proj => proj.Owner);
            }

            var team = GetTeam(user);

            return _context.Project
                .Include(proj => proj.Owner)
                .Where(proj => proj.Owner == team);
        }

        public bool RemoveUserFromProject(ApplicationUser user, Project project)
        {
            

            if (project != null && user != null)
            {
                var userProject = _context.UserProject
                    .Include(up => up.Project)
                    .Include(up => up.User)
                    .FirstOrDefault(userProject => userProject.User == user
                                    && userProject.Project == project);
                _context.Remove(userProject);
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
        
        public async Task<ApplicationUser> GetManagerAsync(Team team)
        {
            var managers = await _userManager.GetUsersInRoleAsync("Manager");
            return managers.FirstOrDefault(user => user.Team == team);
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
        public async Task<IEnumerable<Team>> GetAllTeamsByUser(ApplicationUser user)
        {
            if(await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return _context.Team;
            }
           return new[] { GetTeam(user) };
        }

        public async Task<IEnumerable<UserProject>> GetGetAllUserProjectsByUserAsync(ApplicationUser user)
        {
            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return GetAllUserProjects();
            }
            else if (await _userManager.IsInRoleAsync(user, "Manager"))
            {
                var team = GetTeam(user);
                var projectsByTeam = _context.Project
                    .Include(proj => proj.Owner)
                    .Where(proj => proj.Owner == team);

                return GetAllUserProjects()
                    .Where(userProj => projectsByTeam.Contains(userProj.Project));
            }
            return GetAllUserProjects().Where(userProj => userProj.User == user);
        }

        public void AddProject(Project newProject)
        {
            _context.Add(newProject);
            _context.SaveChanges();
        }

        public void DeleteProject(Project toDelete)
        {
            _context.Remove(toDelete);
            _context.SaveChanges();
        }

        public bool AssignTeamToProject(Team team, Project project)
        {
            if(team != null && project != null)
            {
                if(project.Owner == null)
                {
                    _context.Update(project);
                    project.Owner = team;
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool RemoveTeamFromProject(Team team, Project project)
        {
            if (team != null && project != null)
            {
                if(project.Owner == team)
                {
                    _context.Update(project);
                    project.Owner = null;
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public ApplicationUser GetUserByProject(Project project)
        {
            return _context.UserProject
                .Include(up => up.Project)
                .Include(up => up.User)
                .FirstOrDefault(up => up.Project == project)
                .User;
        }
    }
}
