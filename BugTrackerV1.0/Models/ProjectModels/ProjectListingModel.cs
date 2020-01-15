using System;

namespace BugTracker.Models.ProjectModels
{
    public class ProjectListingModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Team { get; set; }
        public string AssignedTo { get; set; }
    }
}
