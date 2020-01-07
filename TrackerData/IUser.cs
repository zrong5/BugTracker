using System.Threading.Tasks;
using BugTracker.Data.Models;

namespace BugTracker.Data
{
    public interface IUser
    {
        public Task<string> GetAllRolesAsync(ApplicationUser user, char deliminator);
        public string GetTeamName(ApplicationUser user);
        public Task<bool> IsUserUniqueAsync(ApplicationUser user);
    }
}
