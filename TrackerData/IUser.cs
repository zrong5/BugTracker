using System.Threading.Tasks;
using TrackerData.Models;

namespace TrackerData
{
    public interface IUser
    {
        public Task<string> GetAllRoles(ApplicationUser user, char deliminator);
        public string GetTeamName(ApplicationUser user);
    }
}
