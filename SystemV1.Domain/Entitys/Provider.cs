using System.Collections.Generic;

namespace SystemV1.Domain.Entitys
{
    public class Provider : People
    {
        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
    }
}