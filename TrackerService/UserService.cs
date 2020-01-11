using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> _userManager;
        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> CreateUniqueUsernameAsync(string firstName, string lastName)
        {
            var end = lastName.Length >= 6 ? 6 : lastName.Length;
            var name = firstName[0] + lastName.Substring(0, end);
            var random = new Random();
            var randNum = random.Next(1, 100).ToString();
            while (await _userManager.FindByNameAsync(name + randNum) != null)
            {
                randNum = random.Next(1, 100).ToString();
            }
            return name + randNum;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _userManager.Users.Include(user => user.Team);
        }

        public async Task<string> GetAllRolesAsync(ApplicationUser user, char deliminator)
        {
            // concatenate all role names assign to user 
            var currentRoles = await _userManager.GetRolesAsync(user);
            var concatRoles = "";
            if (currentRoles.Any())
            {
                foreach (var role in currentRoles)
                {
                    concatRoles += role + ", ";
                }
                // remove empty spaces and erase last ',' character
                concatRoles = concatRoles.Trim();
                concatRoles = concatRoles[0..^1];
            }
            return concatRoles;
        }

        public string GetTeamName(ApplicationUser user)
        {
            try
            {
                return _userManager.Users
                    .Include(user => user.Team)
                    .FirstOrDefault(u => u.Id == user.Id)
                    .Team.Name;
            }
            catch (NullReferenceException)
            {
                return "";
            }
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
