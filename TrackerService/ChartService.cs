using BugTracker.Data;
using BugTracker.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Service
{
    public class ChartService : IChart
    {
        private readonly TrackerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChartService(TrackerContext context,
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        private async Task<IEnumerable<Bug>> AllBugsByUserAsync(ApplicationUser user)
        {
            var allBugs = _context.Bug
                .Include(bug => bug.Status)
                .Include(bug => bug.ProjectAffected)
                .Include(bug => bug.Owner)
                .Include(bug => bug.LogDetail)
                .Include(bug => bug.Urgency)
                .Include(bug => bug.CreatedBy)
                .Include(bug => bug.ClosedBy);

            if (await _userManager.IsInRoleAsync(user, "Admin"))
            {
                return allBugs;
            }
            else if(await _userManager.IsInRoleAsync(user, "Manager"))
            {
                var team = user.Team;
                return allBugs.Where(bug => bug.AssignedTo.Team == team);
            }
            return allBugs.Where(bug => bug.AssignedTo == user);
        }  

        public async Task<ICollection<MonthlyGraphModel>> GetBugByMonthListAsync(ApplicationUser user)
        {
            var allBugs = await AllBugsByUserAsync(user);

            var modelList = new List<MonthlyGraphModel>();
            for (var i = 0; i < 12; ++i)
            {
                modelList.Add(new MonthlyGraphModel
                {
                    MonthName = DateTimeFormatInfo.CurrentInfo.GetAbbreviatedMonthName(i+1),
                    NumberOfBugs = 0
                });
            }

            foreach (var bug in allBugs)
            {
                if ((bug.CreatedOn.Year == DateTime.Now.Year))
                {
                    var month = bug.CreatedOn.Month;
                    modelList[month - 1].NumberOfBugs++;
                }
            }
            return modelList;
        }

        public async Task<ICollection<StatusGraphModel>> GetBugByStatusListAsync(ApplicationUser user)
        {
            var allBugs = await AllBugsByUserAsync(user);

            var modelList = new List<StatusGraphModel>();
            var allStatus = _context.Status;

            // populate dicitonary to create mapping between status and number of bugs
            var statusMap = new Dictionary<string, int>();
            foreach (var status in allStatus)
            {
                statusMap[status.Name] = 0;
            }
            foreach (var bug in allBugs)
            {
                statusMap[bug.Status.Name]++;
            }

            // populate into list as single object
            foreach (var pair in statusMap)
            {
                modelList.Add(new StatusGraphModel
                {
                    StatusName = pair.Key,
                    NumberOfBugs = pair.Value
                });
            }
            return modelList;
        }

        public async Task<ICollection<UrgencyGraphModel>> GetBugByUrgencyListAsync(ApplicationUser user)
        {
            var allBugs = await AllBugsByUserAsync(user);
            var modelList = new List<UrgencyGraphModel>();
            var allUrgency = _context.Urgency;

            var urgencyMap = new Dictionary<string, int>();
            foreach (var urgency in allUrgency)
            {
                urgencyMap[urgency.Level] = 0;
            }
            foreach (var bug in allBugs)
            {
                urgencyMap[bug.Urgency.Level]++;
            }
            foreach (var pair in urgencyMap)
            {
                modelList.Add(new UrgencyGraphModel
                {
                    UrgencyName = pair.Key,
                    NumberOfBugs = pair.Value
                });
            }

            return modelList;
        }
    }
}
