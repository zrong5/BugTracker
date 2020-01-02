using System.ComponentModel.DataAnnotations;

namespace TrackerData.Models
{
    public class ProcessLog
    {
        [Required]
        public int Id { get; set; }
        public string Detail { get; set; }
    }
}
