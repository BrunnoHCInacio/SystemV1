using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Mappings
{
    public class ProductItemMapping : IEntityTypeConfiguration<ProductItem>
    {
        public void Configure(EntityTypeBuilder<ProductItem> builder)
        {
            builder.HasKey(pi => pi.Id);
            builder.Property(pi => pi.ImageZip).HasColumnType("varchar(1024)");
            builder.Property(pi => pi.IsAvailable);
            builder.Property(pi => pi.IsSold);
            builder.Property(pi => pi.IsActive);
            builder.Property(pi => pi.Modelo)
                   .HasColumnType("varchar(1024)");
            builder.Property(pi => pi.Value)
                   .IsRequired()
                   .HasColumnType("decimal(7,2)");
        }
    }
}