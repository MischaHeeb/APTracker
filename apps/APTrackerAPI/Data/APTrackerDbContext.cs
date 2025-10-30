using APTrackerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace APTrackerAPI.Data
{
    public class APTrackerDbContext : DbContext
    {
        public APTrackerDbContext(DbContextOptions<APTrackerDbContext> options) : base(options) { }

        public DbSet<Attraction> Attractions { get; set; }
        public DbSet<WaitingTime> WaitingTime { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // To improve performance, we'll set two indexes on waiting time.
            modelBuilder.Entity<WaitingTime>()
                .HasIndex(wt => wt.AttractionId);

            modelBuilder.Entity<WaitingTime>()
                .HasIndex(wt => wt.Timestamp);
        }

    }
}
