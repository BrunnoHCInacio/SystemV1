using System;

namespace SystemV1.Domain.Entitys
{
    public abstract class Entity
    {
        public Entity()
        {
            Id = Id == null ? Guid.NewGuid() : Id;
        }
        protected Guid Id { get; set; }
        protected DateTime DateRegister { get; set; }
        protected DateTime? DateChange { get; set; }
        protected int IdUserRegister { get; set; }
        protected int IdUserChange { get; set; }
        public bool IsActive { get; set; }
    }
}