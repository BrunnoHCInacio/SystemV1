using System;
using System.Collections.Generic;
using System.Text;

namespace SystemV1.Domain.Entitys
{
    public class Product:Entity
    {
        public string Name { get; set; }
        
        public IEnumerable<ProductItem> ProductItems { get; set; }

    }
}
