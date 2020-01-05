using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TrackerData.Models;

namespace TrackerData
{
    public class Team
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<ApplicationUser> TeamMembers { get; set; }
    }
}
