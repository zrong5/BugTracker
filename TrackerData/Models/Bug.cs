using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TrackerData.Models;

namespace TrackerData
{
    [Table("Bug")]
    public class Bug
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "DateTime")]
        public DateTime CreatedOn { get; set; }
        [Column(TypeName = "DateTime")]
        public DateTime? ClosedOn { get; set; }
        [Required]
        public ApplicationUser CreatedBy { get; set; }
        public ApplicationUser ClosedBy { get; set; }
        [Required]
        public Project ProjectAffected { get; set; }
        public Team Owner { get; set; }
        [Required]
        public Urgency Urgency { get; set; }
        [Required]
        public Status Status { get; set; }
        public ProcessLog LogDetail { get; set; }
    }
}
