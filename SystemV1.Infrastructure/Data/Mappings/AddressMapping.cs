using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Mappings
{
    public class AddressMapping : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Street).IsRequired().HasColumnType("varchar(1024)");
            builder.Property(a => a.Number).HasColumnType("varchar(1024)");
            builder.Property(a => a.Complement).HasColumnType("varchar(1024)");
            builder.Property(a => a.District).IsRequired().HasColumnType("varchar(1024)");
            builder.HasOne(a => a.City);
            builder.Property(a => a.IsActive);
            builder.ToTable("Addresses");
        }
    }
}