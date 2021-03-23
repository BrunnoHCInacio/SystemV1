using System.Collections.Generic;

namespace SystemV1.Domain.Entitys
{
    public class Client : People
    {
        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
    }
}