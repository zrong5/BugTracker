using System.ComponentModel.DataAnnotations;

namespace TrackerData
{
    public class Team
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
