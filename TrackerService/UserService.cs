﻿using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Data;
using BugTracker.Data.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System;

namespace BugTracker.Service
{
    public class UserService : IUser
    {
        private readonly TrackerContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(TrackerContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public void AssignUserToProject(ApplicationUser user, string projectName)
        {
            _context.Update(user);
            var project = _context.Project
                .FirstOrDefault(project => project.Name == projectName);
            user.Project = project;
            _context.SaveChanges();
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.Users
                .Include(user => user.Project)
                .Include(user => user.Team);
        }

        public IEnumerable<Project> GetAllProjects()
        {
            return _context.Project;
        }

        public async Task<string> GetAllRolesAsync(ApplicationUser user, char deliminator)
        {
            // concatenate all role names assign to user 
            var currentRoles = await _userManager.GetRolesAsync(user);
            var concatRoles = "";
            foreach (var role in currentRoles)
            {
                concatRoles += role + ", ";
            }
            // remove empty spaces and erase last ',' character
            concatRoles = concatRoles.Trim();
            concatRoles = concatRoles[0..^1];
            return concatRoles;
        }

        public string GetTeamName(ApplicationUser user)
        {
            var teamId = user.TeamId;
            var teamName = _context.Team.FirstOrDefault(team => team.Id == teamId).Name;
            return teamName;
        }

        public async Task<bool> IsUserUniqueAsync(ApplicationUser user)
        {
            var emailUnique = await _userManager.FindByNameAsync(user.UserName);
            var usernameUnique = await _userManager.FindByEmailAsync(user.Email);
            if(emailUnique == null && usernameUnique == null)
            {
                return true;
            }
            return false;
        }
    }
}
