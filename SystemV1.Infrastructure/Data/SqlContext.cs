using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Entitys.Audit;

namespace SystemV1.Infrastructure.Data
{
    public class SqlContext : DbContext, IDisposable
    {
        public SqlContext()
        { }

        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }

        public DbSet<Client> Client { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductItem> ProductItem { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<EntityAudit> EntityAudit { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AuditEntities();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public void AuditEntities()
        {
            var auditEntries = new List<EntityAudit>();
            foreach (var entry in ChangeTracker.Entries())
            {
                var options = new JsonSerializerOptions(JsonSerializerDefaults.Web)
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true
                };

                var entitySerialized = JsonSerializer.Serialize(entry.Entity, options);
                var entityaudited = new EntityAudit(entitySerialized,
                                                    GetStateEntry(entry),
                                                    DateTime.Now,
                                                    Guid.NewGuid(),
                                                    entry.Entity.GetType().Name);

                auditEntries.Add(entityaudited);
            }

            foreach (var auditEntry in auditEntries)
            {
                EntityAudit.Add(auditEntry);
            }
        }

        private EnumTypeOperation GetStateEntry(EntityEntry entry)
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    return EnumTypeOperation.DELETE;

                case EntityState.Modified:
                    return EnumTypeOperation.UPDATE;

                case EntityState.Added:
                    return EnumTypeOperation.REGISTER;
            }

            return EnumTypeOperation.REGISTER;
        }
    }
}