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
            var productItemFixture = new ProductItemTestFixture();
            return new
            {
                Id = Guid.NewGuid(),
                Name = faker.Commerce.ProductName(),

                ProductItems = new List<dynamic>
                {
                    productItemFixture.GenerateProductExpected(),
                    productItemFixture.GenerateProductExpected(),
                    productItemFixture.GenerateProductExpected(),
                    productItemFixture.GenerateProductExpected()
                }
            };
        }
        public List<Product> GenerateProduct(int quantity, bool registerActive = true)
        {
            var productItemFixture = new ProductItemTestFixture();
            var product = new Faker<Product>()
                .CustomInstantiator(f =>
                                    new Product(Guid.NewGuid(), f.Commerce.ProductName()))
                .FinishWith((f, p) =>
                {
                    if(!registerActive) p.DisableRegister();
                    p.AddProductItems(productItemFixture.GenerateProductItem(5));
                });

            return product.Generate(quantity);
        }

        public List<Product> GenerateProductWithItems(int quantity, bool registerActive = true)
        {
            var productItemFixture = new ProductItemTestFixture();
            var product = new Faker<Product>()
                .CustomInstantiator(f =>
                                    new Product(Guid.NewGuid(), f.Commerce.ProductName()))
                .FinishWith((f, p) =>
                {
                    if (!registerActive)
                    {
                        p.DisableRegister();
                        p.AddProductItems(productItemFixture.GenerateProductItem(3, registerActive));
                    }

                    p.AddProductItems(productItemFixture.GenerateProductItem(3));
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

        public Product GenerateValidProductDisabled()
        {
            return GenerateProduct(1, false).FirstOrDefault();
        }

        public void Dispose()
        {
        }
    }
}