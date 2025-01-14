using Microsoft.EntityFrameworkCore;
using Unilever.CDExcellent.API.Models;

namespace Unilever.CDExcellent.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<AreaUser> AreaUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite key for AreaUser
            modelBuilder.Entity<AreaUser>()
                .HasKey(au => new { au.AreaId, au.UserId });

            // Configure relationships for AreaUser
            modelBuilder.Entity<AreaUser>()
                .HasOne(au => au.Area)
                .WithMany(a => a.AreaUsers)
                .HasForeignKey(au => au.AreaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AreaUser>()
                .HasOne(au => au.User)
                .WithMany(u => u.AreaUsers)
                .HasForeignKey(au => au.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete on User

            // Configure relationships for Distributor
            modelBuilder.Entity<Distributor>()
                .HasOne(d => d.Area)
                .WithMany(a => a.Distributors)
                .HasForeignKey(d => d.AreaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure required fields and lengths
            modelBuilder.Entity<Area>()
                .Property(a => a.Code)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<Area>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Distributor>()
                .Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Distributor>()
                .Property(d => d.Email)
                .IsRequired()
                .HasMaxLength(150);
        }
    }
}
