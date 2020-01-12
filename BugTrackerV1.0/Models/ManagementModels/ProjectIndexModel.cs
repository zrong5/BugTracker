using System.Collections.Generic;

namespace BugTracker.Models.ManagementModels
{
    public class ProjectIndexModel
    {
        public ProjectAssignmentUpdateModel UpdateModel { get; set; }
        public ProjectCreateModel CreateModel { get; set; }
        public IEnumerable<ProjectListingModel> UserProjects { get; set; }
        public IEnumerable<string> Users { get; set; }
        public IEnumerable<string> Projects { get; set; }
        public IEnumerable<string> Teams { get; set; }
    }
    public class ProjectAssignmentUpdateModel
    {
        public string Username { get; set; }
        public string ProjectName { get; set; }
    }
    public class ProjectCreateModel
    {
        public string ProjectName { get; set; }
        public string Team { get; set; }
        public string Description { get; set; }
    }
}
