using System.ComponentModel.DataAnnotations;

namespace BugTracker.Models.IdentityModels
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
