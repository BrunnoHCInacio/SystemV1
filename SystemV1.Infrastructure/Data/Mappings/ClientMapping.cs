using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Mappings
{
    public class ClientMapping : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasColumnType("varchar(1024)");

            builder.Property(c => c.Document)
                   .IsRequired()
                   .HasColumnType("varchar(1024)");

            builder.HasMany(c => c.Addresses)
                   .WithOne(a => a.Client)
                   .HasForeignKey(a => a.ClientId);

            builder.HasMany(c => c.Contacts)
                   .WithOne(ct => ct.Client)
                   .HasForeignKey(ct => ct.ClientId);
            builder.Property(c => c.IsActive);
            builder.ToTable("Clients");
        }
    }
}