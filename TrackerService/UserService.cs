using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerData;
using TrackerData.Models;

namespace TrackerService
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
        public async Task<string> GetAllRoles(ApplicationUser user, char deliminator)
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
    }
}
