using System;
using System.Collections.Generic;
using TrackerData;

namespace BugTrackerV1._0.Models.BugModels
{
    public class BugDetailModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Team { get; set; }
        public string Project { get; set; }
        public string Description { get; set; }
        public string Urgency { get; set; }
        public string ProgressLog { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ClosedOn { get; set; }
        public IEnumerable<Status> StatusOptions { get; set; }
        public BugUpdateModel UpdateDetail { get; set; }
    }
    public class BugUpdateModel
    {
        public string NewStatus { get; set; }
        public int Id { get; set; }
        public string UpdateToLog { get; set; }
    }
}
