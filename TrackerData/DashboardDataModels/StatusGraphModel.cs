using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Data
{
    [NotMapped]
    public class StatusGraphModel
    {
        public string StatusName { get; set; }
        public int NumberOfBugs { get; set; }
    }
}
