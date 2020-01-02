using System.ComponentModel.DataAnnotations;

namespace TrackerData
{
    public class Status
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
