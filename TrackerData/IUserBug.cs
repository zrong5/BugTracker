using BugTracker.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BugTracker.Data
{
    public interface IUserBug
    {
        Task<IEnumerable<Bug>> GetAllBugsByUserAsync(ApplicationUser user);
        IEnumerable<ApplicationUser> GetAllTeamMembers(ApplicationUser manager);
        bool AssignUserToProject(ApplicationUser user, string projectName);
        bool RemoveUserFromProject(ApplicationUser user, string projectName);
        IEnumerable<UserProject> GetAllUserProjects();
        bool AssignUserToTeam(ApplicationUser user, string teamName);
        void AddTeam(Team newTeam);
        void DeleteTeam(Team toDelete);
    }
}
