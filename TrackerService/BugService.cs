﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using BugTracker.Data;
using BugTracker.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

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
                .Include(bug => bug.ClosedBy)
                .Include(bug => bug.AssignedTo);
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
            if (projectName == null) return null;
            return GetAllProjects()
                .FirstOrDefault(proj => proj.Name == projectName);
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

        public void Update(int bugId, string toAppend, string statusName, string assignTo, string project)
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
            if(project != null)
            {
                _context.Update(bug);
                bug.ProjectAffected = GetProjectByName(project);
            }
            if(assignTo != null)
            {
                _context.Update(bug);
                bug.AssignedTo = _context.Users.FirstOrDefault(user => user.UserName == assignTo);
                if(statusName == "Open")
                {
                    bug.Status = GetStatusByName("Assigned");
                    _context.SaveChanges();
                    return;
                }
            }
            // check if update is nessecary
            if (newStatus != null && bug.Status != newStatus)
            {
                _context.Update(bug);
                bug.Status = newStatus;
            }
            _context.SaveChanges();
        }

        public Project GetProjectById(Guid Id)
        {
            return _context.Project
                .Include(proj => proj.Owner)
                .FirstOrDefault(proj => proj.Id == Id);
        }
    }
}
