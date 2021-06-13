using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var faker = new Faker("pt_BR");
            return new
            {
                Id = Guid.NewGuid(),
                Name = faker.Commerce.ProductName()
            };
        }

        public List<Product> GenerateProduct(int quantity)
        {
            var productItemFixture = new ProductItemTestFixture();
            var product = new Faker<Product>()
                .CustomInstantiator(f =>
                                    new Product(Guid.NewGuid(), f.Commerce.ProductName()))
                .FinishWith((f, p) =>
                {
                    p.AddProductItems(productItemFixture.GenerateProduct(3));
                });

            return product.Generate(quantity);
        }

        public Product GenerateValidProduct()
        {
            return GenerateProduct(1).FirstOrDefault();
        }

        public Product GenerateInvalidProduct()
        {
            return new Product(Guid.NewGuid(), "");
        }

        public void Dispose()
        {
        }
    }
}