using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemV1.Domain.Validations;

namespace SystemV1.Domain.Entitys
{
    public class Product : Entity
    {
        public Product(Guid id,
                       string name)
        {
            ProductItems = new List<ProductItem>();
            Id = id;
            Name = name;
        }

        public string Name { get; private set; }
        public Provider Provider { get; private set; }

        public List<ProductItem> ProductItems { get; private set; }

        public void SetProvider(Provider provider)
        {
            Provider = provider;
        }

        public void AddProductItems(List<ProductItem> productItems)
        {
            //if(productItems.Select(pi=>pi.Validate))

            ProductItems.AddRange(productItems);
        }

        public ValidationResult ValidateProduct()
        {
            return new ProductValidation().Validate(this);
        }
    }
}