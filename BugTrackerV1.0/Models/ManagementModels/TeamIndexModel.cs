using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.ManagementModels
{
    public class TeamIndexModel
    {
        public IEnumerable<TeamListingModel> UserTeams { get; set; }
        public TeamAssignmentUpdateModel UpdateModel { get; set; }
        public List<string> Usernames { get; set; }
        public List<string> Teams { get; set; }
    }
    public class TeamAssignmentUpdateModel
    {
        public string Username { get; set; }
        public string Team { get; set; }
    }
}
