using Domain.Aggregates.Customer;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Context
{

    // Add-Migration -context ApplicationDbContext Initial -OutputDir EF/Migrations
    // update-database -context ApplicationDbContext

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // کانفیگ‌های اختصاصی Entityها اینجا قرار می‌گیرند
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
