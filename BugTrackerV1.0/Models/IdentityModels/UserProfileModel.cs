using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.IdentityModels
{
    public class UserProfileModel
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public string EmailAddress { get; set; }
        public string Team { get; set; }
    }
}
