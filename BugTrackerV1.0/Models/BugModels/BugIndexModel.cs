using System.Collections.Generic;

namespace BugTracker.Models.BugModels
{
    public class BugIndexModel
    {
        public IEnumerable<BugListingModel> Bugs { get; set; }
    }
}
