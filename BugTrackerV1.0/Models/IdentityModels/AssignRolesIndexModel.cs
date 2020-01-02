using System.Collections.Generic;

namespace BugTrackerV1._0.Models.IdentityModels
{
    public class AssignRolesIndexModel
    {
        public string User { get; set; }
        public string Role { get; set; }
        public List<string> Usernames { get; set; }
        public List<string> Roles { get; set; }
    }
}
