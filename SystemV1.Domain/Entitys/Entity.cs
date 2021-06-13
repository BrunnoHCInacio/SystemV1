using System;

namespace SystemV1.Domain.Entitys
{
    public abstract class Entity
    {
        public Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public DateTime DateRegister { get; set; }
        public DateTime? DateChange { get; set; }
        public int IdUserRegister { get; set; }
        public int IdUserChange { get; set; }
        public bool IsActive { get; set; }
    }
}