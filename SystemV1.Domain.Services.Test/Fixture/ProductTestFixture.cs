using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemV1.Domain.Entitys;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(ProductCollection))]
    public class ProductCollection : ICollectionFixture<ProductTestFixture>
    {
    }

    public class ProductTestFixture : IDisposable
    {
        public dynamic GenerateProductExpected(bool withProductItems = false)
        {
            var faker = new Faker("pt_BR");
            var productItemFixture = new ProductItemTestFixture();
            var product = new
            {
                Id = Guid.NewGuid(),
                Name = faker.Commerce.ProductName(),
                ProviderId = Guid.NewGuid(),
                ProductItems = withProductItems
                                ? new List<dynamic>
                                {
                                    productItemFixture.GenerateProductItemExpected(),
                                    productItemFixture.GenerateProductItemExpected(),
                                    productItemFixture.GenerateProductItemExpected(),
                                    productItemFixture.GenerateProductItemExpected()
                                }
                                : new List<dynamic>()
            };

            return product;
        }

        public List<Product> GenerateProduct(int quantity,
                                             Guid providerId,
                                             bool withProductItems = true,
                                             bool withValidProductItems = true)
        {
            var productItemFixture = new ProductItemTestFixture();
            var product = new Faker<Product>()
                .CustomInstantiator(f =>
                                    new Product(Guid.NewGuid(),
                                    f.Commerce.ProductName(),
                                    providerId))
                .FinishWith((f, p) =>
                {
                    if (withProductItems)
                    {
                        if (withValidProductItems)
                        {
                            p.AddProductItems(productItemFixture.GenerateProductItem(5, p.Id, p.ProviderId));
                        }
                        else
                        {
                            p.AddProductItems(productItemFixture.GenerateInvalidProductItem(5));
                        }
                    }
                });

            return product.Generate(quantity);
        }

        public Product GenerateValidProduct(Guid providerId, bool withProdutcItems = true, bool withValidItens = true)
        {
            return GenerateProduct(1, providerId, withProdutcItems, withValidItens).FirstOrDefault();
        }

        public Product GenerateInvalidProduct()
        {
            return new Product(Guid.NewGuid(), "", Guid.Empty);
        }

        public void Dispose()
        {
        }
    }
}