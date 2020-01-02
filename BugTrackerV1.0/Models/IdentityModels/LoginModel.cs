using System.ComponentModel.DataAnnotations;

namespace BugTrackerV1._0.Models.IdentityModels
{
    public class LoginModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
