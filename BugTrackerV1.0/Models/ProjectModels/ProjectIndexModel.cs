using System.Collections.Generic;

namespace BugTracker.Models.ProjectModels
{
    public class ProjectIndexModel
    {
        public IEnumerable<ProjectListingModel> ListingModel { get; set; }
    }
}
