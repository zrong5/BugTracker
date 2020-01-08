using BugTracker.Data.Models;
using System.Collections.Generic;

namespace BugTracker.Data
{
    public interface IChart
    {
        ICollection<MonthlyGraphModel> GetBugByMonthList(ApplicationUser user);
        ICollection<StatusGraphModel> GetBugByStatusList(ApplicationUser user);
        ICollection<UrgencyGraphModel> GetBugByUrgencyList(ApplicationUser user);
    }
}
