using System.Threading.Tasks;
using TrackerData.Models;

namespace TrackerData
{
    public interface IUser
    {
        public Task<string> GetAllRolesAsync(ApplicationUser user, char deliminator);
        public string GetTeamName(ApplicationUser user);
        public Task<bool> IsUserUniqueAsync(ApplicationUser user);
    }
}
