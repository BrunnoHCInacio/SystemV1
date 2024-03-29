﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Mappings
{
    public class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Name).IsRequired().HasColumnType("varchar(1024)");

            builder.HasMany(p => p.ProductItems)
                   .WithOne(pi => pi.Product)
                   .HasForeignKey(pi => pi.ProductId);
            builder.HasOne(p => p.Provider);
            builder.ToTable("Products");
        }
    }
}