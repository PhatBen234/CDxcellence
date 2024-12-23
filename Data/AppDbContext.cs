using Microsoft.EntityFrameworkCore;
using Unilever.CDExcellent.API.Models;

namespace Unilever.CDExcellent.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            // No need to initialize DbSet properties here
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
