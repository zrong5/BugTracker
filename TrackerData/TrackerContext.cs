using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using TrackerData.Models;

namespace TrackerData
{
    public class TrackerContext: IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public TrackerContext(DbContextOptions<TrackerContext> options) : base(options) 
        {

        }
        public DbSet<Bug> Bug { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<ProcessLog> ProcessLog { get; set; }
        public DbSet<Urgency> Urgency { get; set; }
    }
}
