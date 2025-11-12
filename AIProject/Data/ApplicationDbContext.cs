using System;
using System.Threading;
using System.Threading.Tasks;
using AIProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AIProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Gauge> Gauges => Set<Gauge>();

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            ApplyAuditInformation();
            return base.SaveChanges();
        }

        private void ApplyAuditInformation()
        {
            var utcNow = DateTime.UtcNow;
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Customer customer)
                {
                    if (entry.State == EntityState.Added)
                    {
                        customer.CreatedAtUtc = utcNow;
                        customer.UpdatedAtUtc = utcNow;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        customer.UpdatedAtUtc = utcNow;
                    }
                }
                else if (entry.Entity is Gauge gauge)
                {
                    if (entry.State == EntityState.Added)
                    {
                        gauge.CreatedAtUtc = utcNow;
                        gauge.UpdatedAtUtc = utcNow;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        gauge.UpdatedAtUtc = utcNow;
                    }
                }
            }
        }
    }
}
