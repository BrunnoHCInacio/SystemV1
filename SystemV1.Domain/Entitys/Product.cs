using System;
using System.Collections.Generic;

namespace SystemV1.Domain.Entitys
{
    public class Product : Entity
    {
        public Product(Guid id,
                       string name,
                       Guid providerId)
        {
            ProductItems = new List<ProductItem>();
            Id = id;
            Name = name;
            ProviderId = providerId;
        }

        public string Name { get; private set; }
        public Provider Provider { get; private set; }
        public Guid ProviderId { get; set; }

        public List<ProductItem> ProductItems { get; private set; }

        public void AddProductItems(List<ProductItem> productItems)
        {
            ProductItems.AddRange(productItems);
        }

        public void AddProductItem(ProductItem product)
        {
            ProductItems.Add(product);
        }
    }
}