using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Nxt.Common.Extensions;
using Nxt.Entities.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nxt.Repositories.DataContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        DbSet<Customer> Customers { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            Auditing();

            // After we set all the needed properties
            // we call the base implementation of SaveChangesAsync
            // to actually save our entities in the database
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            Auditing();

            // After we set all the needed properties
            // we call the base implementation of SaveChangesAsync
            // to actually save our entities in the database
            return base.SaveChanges();
        }

        private void Auditing()
        {
            // Get all the entities that inherit from AuditableEntity
            // and have a state of Added or Modified
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            // For each entity we will set the Audit properties
            foreach (var entityEntry in entries)
            {
                // If the entity state is Added let's set
                // the CreatedAt and CreatedBy properties
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedOn = DateTime.UtcNow.GetISTDateTime();
                    ((BaseEntity)entityEntry.Entity).CreatedBy = _httpContextAccessor?.HttpContext?.GetUserEmail() ?? "Systen";
                }
                else
                {
                    // If the state is Modified then we don't want
                    // to modify the CreatedAt and CreatedBy properties
                    // so we set their state as IsModified to false
                    Entry((BaseEntity)entityEntry.Entity).Property(p => p.CreatedOn).IsModified = false;
                    Entry((BaseEntity)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;

                    ((BaseEntity)entityEntry.Entity).UpdatedOn = DateTime.UtcNow.GetISTDateTime();
                    ((BaseEntity)entityEntry.Entity).UpdatedBy = _httpContextAccessor?.HttpContext?.GetUserEmail() ?? "Systen";
                }

                //// In any case we always want to set the properties
                //// ModifiedAt and ModifiedBy
                //((BaseEntity)entityEntry.Entity).UpdatedOn = DateTime.UtcNow.GetISTDateTime();
                //((BaseEntity)entityEntry.Entity).UpdatedBy = _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "Systen";
            }
        }
    }
}
