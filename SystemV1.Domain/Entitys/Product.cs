using System;
using System.Collections.Generic;

namespace SystemV1.Domain.Entitys
{
    public class Product : Entity
    {
        public Product(Guid id,
                       string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; private set; }
        public Provider Provider { get; private set; }

        public List<ProductItem> ProductItems { get; private set; }
    }
}