using API._Entities;
using Microsoft.EntityFrameworkCore;

namespace API._Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
