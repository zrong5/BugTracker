using BugTracker.Data;
using BugTracker.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BugTracker.Service
{
    public class ChartService : IChart
    {
        private readonly TrackerContext _context;

        public ChartService(TrackerContext context)
        {
            _context = context;
        }
        private IQueryable<Bug> AllBugsByUser(ApplicationUser user)
        {
            return _context.Bug
                .Include(bug => bug.AssignedTo)
                .Include(bug => bug.Status)
                .Include(bug => bug.Urgency)
                .Where(bug => bug.AssignedTo == user);
        }
        public ICollection<MonthlyGraphModel> GetBugByMonthList(ApplicationUser user)
        {
            var allBugs = AllBugsByUser(user);

            var modelList = new List<MonthlyGraphModel>();
            for (int i = 0; i < 12; ++i)
            {
                modelList.Add(new MonthlyGraphModel
                {
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1),
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

        public ICollection<StatusGraphModel> GetBugByStatusList(ApplicationUser user)
        {
            var allBugs = AllBugsByUser(user);

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

        public ICollection<UrgencyGraphModel> GetBugByUrgencyList(ApplicationUser user)
        {
            var allBugs = AllBugsByUser(user);
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
