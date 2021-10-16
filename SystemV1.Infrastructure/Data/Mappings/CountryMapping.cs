using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Mappings
{
    public class CountryMapping : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasColumnType("varchar(200)");

            builder.Property(c => c.IsActive);

            builder.HasMany(c => c.States)
                   .WithOne(s => s.Country)
                   .HasForeignKey(s => s.CountryId);

            builder.ToTable("Countries");
        }
    }
}