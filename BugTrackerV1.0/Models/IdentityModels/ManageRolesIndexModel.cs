using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerV1._0.Models.IdentityModels
{
    public class ManageRolesIndexModel
    {
        public string User { get; set; }
        public string Role { get; set; }
        public List<string> Usernames { get; set; }
        public List<string> Roles { get; set; }
    }
}
