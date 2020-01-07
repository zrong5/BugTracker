using System.ComponentModel.DataAnnotations.Schema;

namespace BugTrackerV1._0.Models.BugModels.DashboardDataModels
{
    [NotMapped]
    public class StatusGraphModel
    {
        public string StatusName { get; set; }
        public int NumberOfBugs { get; set; }
    }
}
