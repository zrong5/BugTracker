using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BugTracker.Data;
using BugTracker.Data.Models;

namespace BugTracker.Models.SubmissionModels
{
    public class SubmissionIndexModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Urgency { get; set; }
        public string Team { get; set; }
        public string TeamMember { get; set; }
        public string ProjectAffected { get; set; }
        public SubmissionOptionsModel Options { get; set; }
    }
    public class SubmissionOptionsModel
    {
        public IEnumerable<Team> TeamOptions { get; set; }
        public IEnumerable<ApplicationUser> TeamMemberOptions { get; set; }
        public IEnumerable<Urgency> UrgencyOptions { get; set; }
        public IEnumerable<Project> ProjectOptions { get; set; }
    }
}
