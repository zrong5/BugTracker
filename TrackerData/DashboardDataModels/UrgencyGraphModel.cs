using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Data
{
    [NotMapped]
    public class UrgencyGraphModel
    {
        public string UrgencyName { get; set; }
        public int NumberOfBugs { get; set; }
    }
}
