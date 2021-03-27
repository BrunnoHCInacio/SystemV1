using System.Collections.Generic;

namespace SystemV1.Domain.Entitys
{
    public class Provider : People
    {
        public Provider()
        {
            Addresses = new List<Address>();
            Contacts = new List<Contact>();
        }

        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
    }
}