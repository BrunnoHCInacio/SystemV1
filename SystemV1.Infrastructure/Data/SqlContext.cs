using Microsoft.EntityFrameworkCore;
using System;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data
{
    public class SqlContext : DbContext
    {
        public SqlContext()
        {
        }

        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Country> Countries { get; set; }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity.GetType().GetProperty("DateRegister") != null && entry.State == EntityState.Added)
                {
                    entry.Property("DateRegister").CurrentValue = DateTime.Now;
                }
                if (entry.Entity.GetType().GetProperty("DateChange") != null && entry.State == EntityState.Modified)
                {
                    entry.Property("DateChange").CurrentValue = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }
    }
}