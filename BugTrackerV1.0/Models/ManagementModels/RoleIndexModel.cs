using System.Collections.Generic;

namespace BugTracker.Models.ManagementModels
{
    public class RoleIndexModel
    {
        public RoleAssignmentUpdateModel UpdateModel { get; set; }
        public IEnumerable<RoleListingModel> UserRoles { get; set; }
        public List<string> Usernames { get; set; }
        public List<string> Roles { get; set; }
    }
    public class RoleAssignmentUpdateModel
    {
        public string User { get; set; }
        public string Role { get; set; }
    }
}
