using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Data.Models
{
    [Table("Urgency")]
    public class Urgency
    {
        [Required, Key]
        public Guid Id { get; set; }
        [Required]
        public string Level { get; set; }
        public string Description { get; set; }
    }
}
