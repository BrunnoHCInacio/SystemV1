using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Mappings
{
    public class CityMapping : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(s => s.Name)
                  .IsRequired()
                  .HasColumnType("varchar(200)");

            builder.HasOne(s => s.State)
                   .WithMany(c => c.Cities)
                   .HasForeignKey(s => s.StateId);

            builder.Property(s => s.IsActive);
            builder.ToTable("Cities");
        }
    }
}
