using Microsoft.EntityFrameworkCore;
using EventManagementAPI.Domain.Models;

namespace EventManagementAPI.Infrastructure.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public DbSet<Event> Events { get; set; }
    }
}
