using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTrackerV1._0.Models.BugModels
{
    public class UserProfileModel
    {
        public string UserName { get; set; }
        public string Role { get; set; }
        public string EmailAddress { get; set; }
        public string Team { get; set; }
        public int NumberOfProjectsAssigned { get; set; }
        public int NumberOfBugsResolved { get; set; }
    }
}
