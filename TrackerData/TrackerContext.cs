using Microsoft.EntityFrameworkCore;
using TrackerData.Models;

namespace TrackerData
{
    public class TrackerContext : DbContext
    {
        public TrackerContext(DbContextOptions options) : base(options) { }
        public DbSet<Bug> Bug { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Team> Team { get; set; }
        public DbSet<ProcessLog> ProcessLog{ get; set; }
        public DbSet<Urgency> Urgency { get; set; }
    }
}
