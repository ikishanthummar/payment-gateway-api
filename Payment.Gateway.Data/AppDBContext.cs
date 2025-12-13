using Microsoft.EntityFrameworkCore;
using Payment.Gateway.Data.Entities;

namespace Payment.Gateway.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Transaction>()
                   .HasIndex(t => t.OrderId)
                   .IsUnique(false);
        }
    }
}
