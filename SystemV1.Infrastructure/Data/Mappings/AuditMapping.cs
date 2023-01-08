using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys.Audit;

namespace SystemV1.Infrastructure.Data.Mappings
{
    public class AuditMapping : IEntityTypeConfiguration<EntityAudit>
    {
        public void Configure(EntityTypeBuilder<EntityAudit> builder)
        {
            builder.HasKey(a=>a.Id);
            builder.Property(a=>a.IdUser).IsRequired();
            builder.Property(a => a.AuditedEntity).IsRequired().HasColumnType("varchar(1024)");
            builder.Property(a => a.Date).IsRequired();
            builder.Property(a => a.EntityName).IsRequired().HasColumnType("varchar(250)");
            builder.ToTable("AuditEntities");
        }
    }
}
