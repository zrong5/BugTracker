using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BugTracker.Data.Models
{
    [Table("ApplicationUser")]
    public class ApplicationUser : IdentityUser<Guid>
    {
        public Team Team { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static implicit operator ApplicationUser(ApplicationUser v)
        {
            throw new NotImplementedException();
        }
    }
}
