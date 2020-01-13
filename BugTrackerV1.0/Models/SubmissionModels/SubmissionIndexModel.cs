using System;
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
        public string Developer { get; set; }
        public string ProjectAffected { get; set; }
        public SubmissionOptionsModel Options { get; set; }
    }
    public class SubmissionOptionsModel
    {
        public IEnumerable<string> TeamOptions { get; set; }
        public IEnumerable<string> UrgencyOptions { get; set; }
        public IEnumerable<string> ProjectOptions { get; set; }
        public IEnumerable<UserProjectModel> UserProjectOptions { get; set; }
    }
    public class UserProjectModel
    {
        public string Username { get; set; }
        public string Project { get; set; }
    }
}
