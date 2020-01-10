using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BugTracker.Data;

namespace BugTracker.Models.AccountModels
{
    public class RegisterIndexModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Email { get; set; }
        [Compare("Email", ErrorMessage = "Confirm email doesn't match, Type again !")]
        public string ConfirmEmail { get; set; }
        [Required]
        public string Team { get; set; }
    }
}

