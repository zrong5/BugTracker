using Microsoft.AspNetCore.Identity;

namespace TrackerData.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Team BelongsToTeam { get; set; }
    }
}
