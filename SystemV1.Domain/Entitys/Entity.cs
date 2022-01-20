using System;

namespace SystemV1.Domain.Entitys
{
    public abstract class Entity
    {
        public Entity()
        {
            Id = Guid.NewGuid();
            EnableRegister();
        }

        public Guid Id { get; set; }

        public DateTime DateRegister { get; set; }
        public DateTime? DateChange { get; set; }
        public Guid IdUserRegister { get; set; }
        public Guid IdUserChange { get; set; }
        public bool IsActive { get; set; }

        public void DisableRegister()
        {
            IsActive = false;
        }

        public void EnableRegister()
        {
            IsActive = true;
        }
    }
}