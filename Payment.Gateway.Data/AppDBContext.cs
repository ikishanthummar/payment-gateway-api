using Microsoft.EntityFrameworkCore;
using Payment.Gateway.Data.Entities;

namespace Payment.Gateway.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options) { }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PaymentProviders> PaymentProviders { get; set; }
        public DbSet<WebhookLog> WebhookLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.Id);

                entity.Property(t => t.OrderId)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasIndex(t => t.OrderId)
                      .IsUnique(false);

                entity.Property(t => t.Status)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(t => t.Amount)
                      .HasColumnType("decimal(18,2)");

                entity.HasIndex(t => t.Status);
            });

            builder.Entity<PaymentProviders>(entity =>
            {
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.HasIndex(p => p.Name)
                      .IsUnique();

                entity.Property(p => p.IsActive)
                      .HasDefaultValue(true);
            });

            builder.Entity<WebhookLog>(entity =>
            {
                entity.HasKey(w => w.Id);

                entity.Property(w => w.Payload)
                      .IsRequired();

                entity.Property(w => w.Signature)
                      .HasMaxLength(512);

                entity.Property(w => w.ProcessingStatus)
                      .HasMaxLength(50);

                entity.HasIndex(w => w.TransactionId);

                entity.HasOne(w => w.Transaction)
                      .WithMany(t => t.WebhookLogs)
                      .HasForeignKey(w => w.TransactionId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
