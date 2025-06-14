using InvoiceGenerator.InvoiceGenerator.Infrastructure.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InvoiceGenerator.InvoiceGenerator.Infrastructure
{
    public class InvoiceGeneratorDbContext : IdentityDbContext<User>
    {
        public InvoiceGeneratorDbContext(DbContextOptions<InvoiceGeneratorDbContext> options)
            : base(options) { }

        public DbSet<Invoice> Invoices { get; init; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany(u => u.Invoices)
                .WithOne(i => i.User)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Invoice>()
                .HasMany(i => i.InvoiceItems)
                .WithOne(i => i.Invoice)
                .HasForeignKey(i => i.InvoiceId)
                .OnDelete(DeleteBehavior.NoAction);


            base.OnModelCreating(builder);
        }

    }
}
