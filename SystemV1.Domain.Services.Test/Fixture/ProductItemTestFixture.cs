using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemV1.Domain.Entitys;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(ProductItemCollection))]
    public class ProductItemCollection : ICollectionFixture<ProductItemTestFixture>
    {
    }

    public class ProductItemTestFixture : IDisposable
    {
        public List<ProductItem> GenerateProductItem(int quantity, Guid productId, Guid providerId)
        {
            var product = new Faker<ProductItem>()
                .CustomInstantiator(f => new ProductItem(Guid.NewGuid(),
                                                         f.Commerce.ProductName(),
                                                         f.Random.Decimal(0.00M, 999999999.99M),
                                                         new Product(productId,
                                                                     "",
                                                                     providerId)))
                .FinishWith((f, pi) =>
                {
                    pi.SetProductItemSold();
                });
            return product.Generate(quantity);
        }

        public ProductItem GenerateValidProductItem(Guid productId, Guid providerId)
        {
            return GenerateProductItem(1, productId, providerId).FirstOrDefault();
        }

        public List<ProductItem> GenerateInvalidProductItem(int quantity)
        {
            return new Faker<ProductItem>().CustomInstantiator(f => new ProductItem(new Guid(), null, 0.0M, null)).Generate(quantity);
        }

        public ProductItem GenerateInvalidProductItem()
        {
            return GenerateInvalidProductItem(1).FirstOrDefault();
        }

        public dynamic GenerateProductItemExpected()
        {
            var faker = new Faker("pt_BR");
            return new
            {
                Id = Guid.NewGuid(),
                Modelo = faker.Commerce.ProductName(),
                Value = faker.Random.Decimal(0.00M, 999999999.99M),
                IsSold = false,
                IsAvailable = true,
                ImageZip = "image.jpg"
            };
        }

        public void Dispose()
        {
        }
    }
}