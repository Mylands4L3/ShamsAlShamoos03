


 using Microsoft.EntityFrameworkCore;
using ShamsAlShamoos03.Shared.Entities;

namespace ShamsAlShamoos03.Infrastructure.Persistence.Contexts
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
