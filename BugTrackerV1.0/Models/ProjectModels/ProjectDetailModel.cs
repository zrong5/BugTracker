using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugTracker.Models.ProjectModels
{
    public class ProjectDetailModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Team { get; set; }
        public string AssignedTo { get; set; }
        public string Email { get; set; }
    }
}
