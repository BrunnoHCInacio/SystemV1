using System;

namespace SystemV1.Domain.Entitys
{
    public class Client : Entity
    {
        public Client()
        { }

        public Client(Guid id,
                      Guid peopleId)
        {
            Id = id;
            PeopleId = peopleId;
        }

        public People People { get; private set; }
        public Guid PeopleId { get; private set; }
    }
}