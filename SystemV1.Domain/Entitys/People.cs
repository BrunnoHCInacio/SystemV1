using System.Collections.Generic;

namespace SystemV1.Domain.Entitys
{
    public class People : Entity
    {
        public string Name { get; set; }
        public string Document { get; set; }
        public IEnumerable<PeopleAddress> Addresses { get; set; }
        public IEnumerable<PeopleContact> Contacts { get; set; }
    }
}