using System.Collections.Generic;

namespace BugTracker.Models.ManagementModels
{
    public class RolesIndexModel
    {
        public string User { get; set; }
        public string Role { get; set; }
        public List<string> Usernames { get; set; }
        public List<string> Roles { get; set; }
    }
}
