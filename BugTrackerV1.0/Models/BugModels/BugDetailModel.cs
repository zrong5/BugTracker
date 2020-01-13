using BugTracker.Data.Models;
using System;
using System.Collections.Generic;

namespace BugTracker.Models.BugModels
{
    public class BugDetailModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Team { get; set; }
        public string AssignedTo { get; set; }
        public string Status { get; set; }
        public string Project { get; set; }
        public string Description { get; set; }
        public string Urgency { get; set; }
        public string ProgressLog { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ClosedBy { get; set; }
        public DateTime? ClosedOn { get; set; }
        public IEnumerable<string> StatusOptions { get; set; }
        public IEnumerable<string> DeveloperOptions{ get; set; }
        public IEnumerable<string> ProjectOptions { get; set; }
        public IEnumerable<UserProjectModel> UserProjectOptions { get; set; }
        public BugUpdateModel UpdateDetail { get; set; }
    }
    public class BugUpdateModel
    {
        public string NewStatus { get; set; }
        public string UpdateToLog { get; set; }
        public string AssignedTo { get; set; }
        public string Project { get; set; }
    }
    public class UserProjectModel
    {
        public string Username { get; set; }
        public string Project { get; set; }
    }
}
