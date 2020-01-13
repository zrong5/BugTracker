namespace BugTracker.Models.ManagementModels
{
    public class AssignedProjectListingModel
    {
        public string FullName { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string Project { get; set; }
    }
    public class ProjectListingModel
    {
        public string Manager { get; set; }
        public string Email { get; set; }
        public string Team { get; set; }
    }
}
