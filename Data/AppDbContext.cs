using Microsoft.EntityFrameworkCore;
using Unilever.CDExcellent.API.Models;

namespace Unilever.CDExcellent.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
