using System.Collections.Generic;

namespace SystemV1.Domain.Entitys
{
    public abstract class People : Entity
    {
        public string Name { get; set; }
        public string Document { get; set; }
    }
}