using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Data.Models
{
    [Table("UserProject")]
    public class UserProject
    {
        [Key]
        public Guid Id { get; set; }
        public ApplicationUser User { get; set; }
        public Project Project { get; set; }
    }
}
