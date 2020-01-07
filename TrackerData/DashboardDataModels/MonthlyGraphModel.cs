using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Data
{
    [NotMapped]
    public class MonthlyGraphModel
    {
        public string MonthName { get; set; }
        public int NumberOfBugs { get; set; }
    }
}
