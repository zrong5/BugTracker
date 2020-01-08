using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Data.Models
{
    [Table("ApplicationUser")]
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Guid TeamId { get; set; }
        [ForeignKey("TeamId")]
        public Team Team { get; set; }
        public Project Project { get; set; }
    }
}
