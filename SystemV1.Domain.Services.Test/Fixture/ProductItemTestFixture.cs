﻿using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public List<ProductItem> GenerateProductItem(int quantity, bool registerActive = true)
        {
            var product = new Faker<ProductItem>()
                .CustomInstantiator(f => new ProductItem(Guid.NewGuid(),
                                                         f.Commerce.ProductName(),
                                                         f.Random.Decimal(0.00M, 999999999.99M)))
                .FinishWith((f, pi) =>
                {
                    if (!registerActive) pi.DisableRegister();

                    pi.SetProductItemAvailable();
                    pi.SetProductItemSold();
                });
            return product.Generate(quantity);
        }

        public ProductItem GenerateValidProduct()
        {
            return GenerateProductItem(1).FirstOrDefault();
        }

        public ProductItem GenerateInvalidProduct()
        {
            return new ProductItem(new Guid(), null, 0.0M);
        }

        public ProductItem GenerateValidProductDisabled()
        {
            return GenerateProductItem(1, false).FirstOrDefault();
        }

        public dynamic GenerateProductExpected()
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