using System.Collections.Generic;

namespace SystemV1.Domain.Entitys
{
    public class Product : Entity
    {
        public string Name { get; set; }

        public IEnumerable<ProductItem> ProductItems { get; set; }
    }
}