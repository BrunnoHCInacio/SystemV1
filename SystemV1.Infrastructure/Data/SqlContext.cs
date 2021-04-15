using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
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
            Connection = new SqlConnection(Database.GetDbConnection().ConnectionString);
        }

        public SqlConnection Connection { get; set; }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductItem> ProductItems { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Country> Countries { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity.GetType().GetProperty("DateRegister") != null && entry.State == EntityState.Added)
                {
                    entry.Property("DateRegister").CurrentValue = DateTime.Now;
                    entry.Property("IsActive").CurrentValue = true;
                }
                if (entry.Entity.GetType().GetProperty("DateChange") != null && entry.State == EntityState.Modified)
                {
                    entry.Property("DateChange").CurrentValue = DateTime.Now;
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}