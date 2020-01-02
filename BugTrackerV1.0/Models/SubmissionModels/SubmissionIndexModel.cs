﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TrackerData;
using TrackerData.Models;

namespace BugTrackerV1._0.Models.SubmissionModels
{
    public class SubmissionIndexModel
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Urgency { get; set; }
        [Required]
        public string Team { get; set; }
        [Required]
        public string ProjectAffected { get; set; }
        public SubmissionOptionsModel Options { get; set; }
    }
    public class SubmissionOptionsModel
    {
        public IEnumerable<Team> TeamOptions { get; set; }
        public IEnumerable<Urgency> UrgencyOptions { get; set; }
        public IEnumerable<Project> ProjectOptions { get; set; }
    }
}
