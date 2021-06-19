using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data
{
    public class SqlContext : DbContext, IDisposable
    {
        public SqlContext()
        { }

        public SqlContext(DbContextOptions<SqlContext> options) : base(options)
        {
        }

        private NpgsqlConnection _connection;

        public NpgsqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new NpgsqlConnection(Database.GetDbConnection().ConnectionString);
                    _connection.Open();
                }
                else
                {
                }
                return _connection;
            }
        }

        public DbSet<Client> Client { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<ProductItem> ProductItem { get; set; }
        public DbSet<State> State { get; set; }
        public DbSet<Country> Country { get; set; }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
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

        public override void Dispose()
        {
            base.Dispose();
            if (_connection != null)
            {
                _connection.Close();
            }
        }
    }
}