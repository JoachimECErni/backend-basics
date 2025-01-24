using CRUDApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUDApplication.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<Game> Games{ get; set; }
    }
}
