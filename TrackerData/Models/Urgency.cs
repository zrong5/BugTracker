using System.ComponentModel.DataAnnotations;

namespace TrackerData.Models
{
    public class Urgency
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Level { get; set; }
        public string Description { get; set; }
    }
}
