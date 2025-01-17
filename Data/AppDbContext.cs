﻿using Microsoft.EntityFrameworkCore;
using Unilever.CDExcellent.API.Models.Entities;

namespace Unilever.CDExcellent.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<AreaUser> AreaUsers { get; set; }
        public DbSet<VisitPlan> VisitPlans { get; set; }
        public DbSet<VisitPlanGuest> VisitPlanGuests { get; set; } 
        public DbSet<Notification> Notifications { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình AreaUser
            modelBuilder.Entity<AreaUser>()
                .HasKey(au => new { au.AreaId, au.UserId });

            modelBuilder.Entity<AreaUser>()
                .HasOne(au => au.Area)
                .WithMany(a => a.AreaUsers)
                .HasForeignKey(au => au.AreaId);

            modelBuilder.Entity<AreaUser>()
                .HasOne(au => au.User)
                .WithMany(u => u.AreaUsers)
                .HasForeignKey(au => au.UserId);

            // Cấu hình Distributor
            modelBuilder.Entity<Distributor>()
                .HasOne(d => d.Area)
                .WithMany(a => a.Distributors)
                .HasForeignKey(d => d.AreaId);

            // Cấu hình VisitPlanGuest (Bảng trung gian)
            modelBuilder.Entity<VisitPlanGuest>()
                .HasKey(vpg => new { vpg.VisitPlanId, vpg.GuestId }); // Khóa chính

            modelBuilder.Entity<VisitPlanGuest>()
                .HasOne(vpg => vpg.VisitPlan)
                .WithMany(vp => vp.Guests)
                .HasForeignKey(vpg => vpg.VisitPlanId);

            modelBuilder.Entity<VisitPlanGuest>()
                .HasOne(vpg => vpg.Guest)
                .WithMany()
                .HasForeignKey(vpg => vpg.GuestId);
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Message).IsRequired();
            });
        }
    }
}
