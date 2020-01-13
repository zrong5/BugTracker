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
        Task<IEnumerable<UserProject>> GetGetAllUserProjectsByUserAsync(ApplicationUser user);
        Task<IEnumerable<Team>> GetAllTeamsByUser(ApplicationUser user);
        Task<ApplicationUser> GetManagerAsync(Team team);
        bool AssignUserToProject(ApplicationUser user, Project project);
        bool AssignTeamToProject(Team team, Project project);
        bool RemoveTeamFromProject(Team team, Project project);
        bool RemoveUserFromProject(ApplicationUser user, Project project);
        IEnumerable<UserProject> GetAllUserProjects();
        bool AssignUserToTeam(ApplicationUser user, string teamName);
        void AddTeam(Team newTeam);
        void DeleteTeam(Team toDelete);
        void AddProject(Project newProject);
        void DeleteProject(Project toDelete);
    }
}
