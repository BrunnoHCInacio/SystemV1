using System;
using System.Collections.Generic;
using System.Text;

namespace SystemV1.Domain.Entitys
{
    public class PeopleAddress : Entity
    {
        public int ZipCode { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string District { get; set; }
        public State State { get; set; }
        public Country Country { get; set; }
    }
}