using System;
using System.Collections.Generic;
using System.Text;

namespace SystemV1.Domain.Entitys
{
    public class PeopleContact : Entity
    {
        public string TypeContact { get; set; }
        public string CellPhoneNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}