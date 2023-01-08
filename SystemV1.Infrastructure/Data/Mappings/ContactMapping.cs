using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SystemV1.Domain.Entitys;

namespace SystemV1.Infrastructure.Data.Mappings
{
    public class ContactMapping : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.CellPhoneNumber).HasColumnType("varchar(1024)");
            builder.Property(c => c.PhoneNumber).HasColumnType("varchar(1024)");
            builder.Property(c => c.Ddd).HasColumnType("varchar(1024)");
            builder.Property(c => c.Ddi).HasColumnType("varchar(1024)");
            builder.Property(c => c.Email).HasColumnType("varchar(1024)");
            builder.Property(c => c.TypeContact).HasColumnType("varchar(200)");
            builder.HasOne(c => c.People);
            builder.ToTable("Contacts");
        }
    }
}