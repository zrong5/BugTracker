﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BugTracker.Data.Models;

namespace BugTracker.Data
{
    public interface IUser
    {
        Task<string> GetAllRolesAsync(ApplicationUser user, char deliminator);
        string GetTeamName(ApplicationUser user);
        Task<bool> IsUserUniqueAsync(ApplicationUser user);
        IEnumerable<ApplicationUser> GetAll();
        Task<string> CreateUniqueUsernameAsync(string firstName, string lastName);
    }
}
