using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using BugTracker.Data;
using BugTracker.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Globalization;

namespace BugTracker.Service
{
    public class BugService : IBug
    {
        private readonly TrackerContext _context;
        
        public BugService(TrackerContext context)
        {
            _context = context;
        }

        public int Add(Bug newBug)
        {
            _context.Add(newBug);
            _context.SaveChanges();
            return newBug.Id;
        }

        public IEnumerable<Bug> GetAll()
        {
            return _context.Bug
                .Include(bug => bug.Status)
                .Include(bug => bug.ProjectAffected)
                .Include(bug => bug.Owner)
                .Include(bug => bug.LogDetail)
                .Include(bug => bug.Urgency)
                .Include(bug => bug.CreatedBy)
                .Include(bug => bug.ClosedBy);
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return _context.Project;
        }

        public IEnumerable<Status> GetAllStatus()
        {
            return _context.Status;
        }

        public IEnumerable<Team> GetAllTeams()
        {
            return _context.Team;
        }

        public IEnumerable<Urgency> GetAllUrgencies()
        {
            return _context.Urgency;
        }

        public Bug GetById(int Id)
        {
            return GetAll().FirstOrDefault(bug => bug.Id == Id);
        }

        public ProcessLog CreateEmptyLog()
        {
            ProcessLog p = new ProcessLog()
            {
                Detail = ""
            };
            _context.Add(p);
            _context.SaveChanges();
            return p;
        }

        public Project GetProjectByName(string projectName)
        {
            return GetAllProjects()
                .FirstOrDefault(proj => proj.Name == projectName);
        }

        public Status GetStatusById(Guid Id)
        {
            return GetAllStatus()
                .FirstOrDefault(s => s.Id == Id);
        }

        public Status GetStatusByName(string statusName)
        {
            return GetAllStatus()
                .FirstOrDefault(status => status.Name == statusName);
        }

        public Team GetTeamByName(string teamName)
        {
            return GetAllTeams()
                .FirstOrDefault(team => team.Name == teamName);
        }

        public Urgency GetUrgencyByName(string urgencyLevel)
        {
            return GetAllUrgencies()
                .FirstOrDefault(urgency => urgency.Level == urgencyLevel);
        }

        public void Update(int bugId, string toAppend, string statusName)
        {
            var bug = GetById(bugId);
            var now = DateTime.Now;
            var newStatus = GetStatusByName(statusName);
            
            // dont trigger an update if to append is an empty string
            if (toAppend != null && toAppend != "")
            {
                // format new message
                var newMessage = now + "<br />" + toAppend + "<br /><br />";
                // if log is empty, create. if not, append
                if (bug.LogDetail == null)
                {
                    ProcessLog newLog = new ProcessLog()
                    {
                        Detail = newMessage
                    };
                    bug.LogDetail = newLog;
                    _context.Add(newLog);
                }
                else
                {
                    _context.Update(bug);
                    bug.LogDetail.Detail = bug.LogDetail.Detail + newMessage;
                }
                
            }
            // check if update is nessecary
            if (bug.Status != newStatus)
            {
                _context.Update(bug);
                bug.Status = newStatus;
            }
            _context.SaveChanges();
        }

        public ICollection<MonthlyGraphModel> GetBugByMonthList(ApplicationUser user)
        {
            var allBugs = GetAll().Where(bug => bug.AssignedTo == user);
            var modelList = new List<MonthlyGraphModel>();
            for(int i = 0; i < 12; ++i)
            {
                modelList.Add(new MonthlyGraphModel
                {
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i + 1),
                    NumberOfBugs = 0
                });
            }
            
            foreach(var bug in allBugs)
            {
                if((bug.CreatedOn.Year == DateTime.Now.Year))
                {
                    var month = bug.CreatedOn.Month;
                    modelList[month - 1].NumberOfBugs++;
                }
            }
            return modelList;
        }

        public ICollection<StatusGraphModel> GetBugByStatusList(ApplicationUser user)
        {
            var allBugs = GetAll().Where(bug => bug.AssignedTo == user);
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
            foreach(var pair in statusMap)
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
            var allBugs = GetAll().Where(bug => bug.AssignedTo == user);
            var modelList = new List<UrgencyGraphModel>();
            var allUrgency = _context.Urgency;

            var urgencyMap = new Dictionary<string, int>();
            foreach(var urgency in allUrgency)
            {
                urgencyMap[urgency.Level] = 0;
            }
            foreach(var bug in allBugs)
            {
                urgencyMap[bug.Urgency.Level]++;
            }
            foreach(var pair in urgencyMap)
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
