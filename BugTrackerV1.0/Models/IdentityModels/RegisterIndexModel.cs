﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TrackerData;

namespace BugTrackerV1._0.Models.IdentityModels
{
    public class RegisterIndexModel
    {
        [Required]
        public string Username { get; set; }
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
        public RegisterOptionsModel Options { get; set; }
    }
    public class RegisterOptionsModel
    {
        public IEnumerable<Team> TeamOptions { get; set; }
    }
}