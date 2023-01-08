using System;

namespace SystemV1.Domain.Entitys
{
    public class Provider : Entity
    {
        public Provider(Guid id)
        {
            Id = id;
        }

        public Provider(Guid id, Guid peopleId)
        {
            Id = id;
            PeopleId = peopleId;
        }

        public People People { get; private set; }
        public Guid PeopleId { get; private set; }
    }
}