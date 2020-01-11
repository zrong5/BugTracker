using BugTracker.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BugTracker.Data
{
    public interface IUserBug
    {
        Task<IEnumerable<Bug>> GetAllBugsByUserAsync(ApplicationUser user);
        Task<IEnumerable<ApplicationUser>> GetAllTeamMembersAsync(ApplicationUser manager);
        Task<IEnumerable<Project>> GetAllProjectByUserAsync(ApplicationUser user);
        bool AssignUserToProject(ApplicationUser user, string projectName);
        bool RemoveUserFromProject(ApplicationUser user, string projectName);
        IEnumerable<UserProject> GetAllUserProjects();
        bool AssignUserToTeam(ApplicationUser user, string teamName);
        void AddTeam(Team newTeam);
        void DeleteTeam(Team toDelete);
    }
}
