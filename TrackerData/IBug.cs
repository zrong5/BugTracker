﻿using System.Collections.Generic;
using TrackerData.Models;

namespace TrackerData
{
    public interface IBug
    {
        IEnumerable<Bug> GetAll();
        int Add(Bug newBug);
        Bug GetById(int Id);
        void Update(int bugId, string toAppend, string newStatus);
        Status GetStatusByName(string statusName);
        Team GetTeamByName(string teamName);
        Project GetProjectByName(string projectName);
        Urgency GetUrgencyByName(string urgencyLevel);
        ProcessLog CreateEmptyLog();
        IEnumerable<Status> GetAllStatus();
        IEnumerable<Team> GetAllTeams();
        IEnumerable<Urgency> GetAllUrgencies();
        IEnumerable<Project> GetAllProjects();
    }
}