using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BugTracker.Data.Models;

namespace BugTracker.Data
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
