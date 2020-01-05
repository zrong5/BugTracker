using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackerData.Models;

namespace TrackerData
{
    [Table("Team")]
    public class Team
    {
        [Required, Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public ICollection<ApplicationUser> TeamMembers { get; set; }
    }
}
