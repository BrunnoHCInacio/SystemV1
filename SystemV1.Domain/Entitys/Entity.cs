using System;

namespace SystemV1.Domain.Entitys
{
    public abstract class Entity
    {
        protected int Id { get; set; }
        protected DateTime DateRegister { get; set; }
        protected DateTime? DateChange { get; set; }
        protected int IdUserRegister { get; set; }
        protected int IdUserChange { get; set; }
        protected bool IsActive { get; set; }
    }
}