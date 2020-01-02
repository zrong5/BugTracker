using System.ComponentModel.DataAnnotations;

namespace TrackerData
{
    public class Project
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public Team Owner { get; set; }
    }
}
