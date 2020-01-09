using BugTracker.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BugTracker.Data
{
    public interface IChart
    {
        Task<ICollection<MonthlyGraphModel>> GetBugByMonthListAsync(ApplicationUser user);
        Task<ICollection<StatusGraphModel>> GetBugByStatusListAsync(ApplicationUser user);
        Task<ICollection<UrgencyGraphModel>> GetBugByUrgencyListAsync(ApplicationUser user);
    }
}
