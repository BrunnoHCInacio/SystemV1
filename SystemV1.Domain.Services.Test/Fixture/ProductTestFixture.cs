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
        public List<Product> GenerateProduct(int quantity, 
                                             bool withProductItems = true, 
                                             bool withValidProductItems = true,
                                             bool registerActive = true)
        {
            var productItemFixture = new ProductItemTestFixture();
            var product = new Faker<Product>()
                .CustomInstantiator(f =>
                                    new Product(Guid.NewGuid(), 
                                    f.Commerce.ProductName()))
                .FinishWith((f, p) =>
                {
                    if(!registerActive) p.DisableRegister();

                    if (withProductItems)
                    {
                        if (withValidProductItems)
                        {
                            p.AddProductItems(productItemFixture.GenerateProductItem(5));
                        }
                        else
                        {
                            p.AddProductItems(productItemFixture.GenerateInvalidProductItem(5));
                        }
                    }
                });

            return product.Generate(quantity);
        }

        public Product GenerateValidProduct(bool withProdutcItems = true, bool withValidItens = true)
        {
            return GenerateProduct(1, withProdutcItems, withValidItens).FirstOrDefault();
        }

        public Product GenerateInvalidProduct()
        {
            return new Product(Guid.NewGuid(), "");
        }

        public Product GenerateValidProductDisabled()
        {
            return GenerateProduct(1, true, true, false).FirstOrDefault();
        }

        public void Dispose()
        {
        }
    }
}