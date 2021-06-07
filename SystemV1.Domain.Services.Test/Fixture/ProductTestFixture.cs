using Bogus;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Test.DomainTests;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(ProductCollection))]
    public class ProductCollection : ICollectionFixture<ProductTestFixture>
    {
    }

    public class ProductTestFixture : IDisposable
    {
        public dynamic GenerateProductExpected()
        {
            return new object
            {
            }
        }

        public List<Product> GenerateProduct(int quantity)
        {
            var product = new Faker<Product>()
                .CustomInstantiator(f =>
                                    new Product(Guid.NewGuid(), f.Commerce.ProductName()));
            return product.Generate(quantity);
        }

        public List<Product> GenerateProduct(int quantity)
        {
            var product = new Faker<Product>()
                .CustomInstantiator(f =>
                                    new Product(Guid.NewGuid(), f.Commerce.ProductName()));
            return product.Generate(quantity);
        }

        public void Dispose()
        {
        }
    }
}