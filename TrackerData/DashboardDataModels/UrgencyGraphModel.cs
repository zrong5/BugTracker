using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerV1._0.Models.BugModels.DashboardDataModels
{
    [NotMapped]
    public class UrgencyGraphModel
    {
        public string UrgencyName { get; set; }
        public int NumberOfBugs { get; set; }
    }
}
