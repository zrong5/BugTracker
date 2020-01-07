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
        [Required, Key]
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
        public ApplicationUser AssignedTo { get; set; }
        public Project ProjectAffected { get; set; }
        public Team Owner { get; set; }
        public Urgency Urgency { get; set; }
        public Status Status { get; set; }
        public ProcessLog LogDetail { get; set; }
    }
}
