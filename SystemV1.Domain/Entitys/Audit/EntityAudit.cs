using System;

namespace SystemV1.Domain.Entitys.Audit
{
    public class EntityAudit : Entity
    {
        public EntityAudit(string auditedEntity,
                           EnumTypeOperation typeOperation,
                           DateTime date,
                           Guid idUser,
                           string entityName)
        {
            AuditedEntity = auditedEntity;
            TypeOperation = typeOperation;
            Date = date;
            IdUser = idUser;
            EntityName = entityName;
        }

        public string AuditedEntity { get; private set; }
        public EnumTypeOperation TypeOperation { get; private set; }
        public DateTime Date { get; private set; }
        public Guid IdUser { get; private set; }
        public string EntityName { get; set; }
    }
}