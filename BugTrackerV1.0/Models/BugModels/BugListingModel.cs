﻿using System;

namespace BugTracker.Models.BugModels
{
    public class BugListingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Urgency { get; set; }
        public string AssignedTo { get; set; }
        public string Team { get; set; }
        public string Status { get; set; }
        public string Project { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
