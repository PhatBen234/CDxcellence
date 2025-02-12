using Microsoft.EntityFrameworkCore;
using Unilever.CDExcellent.API.Models.Entities;

namespace Unilever.CDExcellent.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<OtpCode> OtpCodes { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<AreaUser> AreaUsers { get; set; }
        public DbSet<VisitPlan> VisitPlans { get; set; }
        public DbSet<VisitPlanGuest> VisitPlanGuests { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }

        // CMS Tables
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // AreaUser Relationship
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

            // Distributor Relationship
            modelBuilder.Entity<Distributor>()
                .HasOne(d => d.Area)
                .WithMany(a => a.Distributors)
                .HasForeignKey(d => d.AreaId);

            // VisitPlanGuest Relationship
            modelBuilder.Entity<VisitPlanGuest>()
                .HasKey(vpg => new { vpg.VisitPlanId, vpg.GuestId });

            modelBuilder.Entity<VisitPlanGuest>()
                .HasOne(vpg => vpg.VisitPlan)
                .WithMany(vp => vp.Guests)
                .HasForeignKey(vpg => vpg.VisitPlanId);

            modelBuilder.Entity<VisitPlanGuest>()
                .HasOne(vpg => vpg.Guest)
                .WithMany()
                .HasForeignKey(vpg => vpg.GuestId);

            // Notification
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(n => n.Id);
                entity.Property(n => n.Message).IsRequired();
            });

            // UserTask Relationship
            modelBuilder.Entity<UserTask>()
                .HasOne(t => t.VisitPlan)
                .WithMany(vp => vp.UserTasks)
                .HasForeignKey(t => t.VisitPlanId);

            modelBuilder.Entity<UserTask>()
                .HasOne(t => t.Assignee)
                .WithMany()
                .HasForeignKey(t => t.AssigneeId);

            // Article Relationships
            modelBuilder.Entity<Article>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Articles)
                .HasForeignKey(a => a.CategoryId);

            modelBuilder.Entity<Article>()
                .HasOne(a => a.Author)
                .WithMany(u => u.Articles)
                .HasForeignKey(a => a.AuthorId)
                .OnDelete(DeleteBehavior.Restrict); // Tránh xóa User sẽ gây lỗi khóa ngoại

            // Comment Relationships
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Article)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.ArticleId)
                .OnDelete(DeleteBehavior.Cascade); // Xóa Article thì xóa Comment

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Không cho phép xóa User kéo theo xóa Comment
        }
    }
}
