


 using Microsoft.EntityFrameworkCore;
using ShamsAlShamoos01.Shared.Entities;

namespace ShamsAlShamoos01.Infrastructure.Persistence.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<HistoryRegisterKala01> HistoryRegisterKala01 { get; set; }
    }
}
