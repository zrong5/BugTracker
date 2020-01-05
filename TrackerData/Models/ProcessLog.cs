using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackerData.Models
{
    [Table("ProcessLog")]
    public class ProcessLog
    {
        [Required, Key]
        public Guid Id { get; set; }
        public string Detail { get; set; }
    }
}
