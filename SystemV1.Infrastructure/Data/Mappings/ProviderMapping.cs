using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Mappings
{
    public class ProviderMapping : IEntityTypeConfiguration<Provider>
    {
        public void Configure(EntityTypeBuilder<Provider> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasColumnType("varchar(1024)");

            builder.Property(c => c.Document)
                   .IsRequired()
                   .HasColumnType("varchar(1024)");

            builder.HasMany(c => c.Addresses)
                   .WithOne(a => a.Provider)
                   .HasForeignKey(a => a.ProviderId);

            builder.HasMany(c => c.Contacts)
                   .WithOne(ct => ct.Provider)
                   .HasForeignKey(ct => ct.ProviderId);

            builder.ToTable("Providers");
        }
    }
}