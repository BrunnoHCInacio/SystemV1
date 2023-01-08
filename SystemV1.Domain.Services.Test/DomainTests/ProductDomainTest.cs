using System;
using System.Linq;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.DomainTests
{
    [Collection(nameof(ProductCollection))]
    public class ProductDomainTest
    {
        private readonly ProductTestFixture _productTestFixture;

        public ProductDomainTest(ProductTestFixture productTestFixture)
        {
            _productTestFixture = productTestFixture;
        }

        #region Test validate expected data

        [Fact(DisplayName = "Validate set as correct properties for product")]
        [Trait("UnitTests - Entity", "Product")]
        public void Product_NewProduct_ShouldSetCorrectProperties()
        {
            //Arrange
            var productExpected = _productTestFixture.GenerateProductExpected();

            //Act
            var product = new Product(productExpected.Id, productExpected.Name, productExpected.ProviderId);

            //Assert
            Assert.Equal(productExpected.Id, product.Id);
            Assert.Equal(productExpected.Name, productExpected.Name);
        }

        [Fact(DisplayName = "Validate set as correct properties for product item")]
        [Trait("UnitTests - Entity", "Product")]
        public void Product_NewProductWithProductItem_ShouldSetCorrectProperties()
        {
            //Arrange
            var productExpected = _productTestFixture.GenerateProductExpected();

            //Act
            var product = new Product(productExpected.Id, productExpected.Name, productExpected.ProviderId);

            foreach (var item in productExpected.ProductItems)
            {
                var productItem = new ProductItem(item.Id,
                                                  item.Modelo,
                                                  item.Value,
                                                  item.ImageZip);
                productItem.SetProductItemAvailable();

                product.AddProductItem(productItem);
            }

            //Assert
            Assert.Equal(productExpected.Id, product.Id);
            Assert.Equal(productExpected.Name, productExpected.Name);

            //TODO: Adicionar teste para as propriedade de log.

            foreach (var item in product.ProductItems)
            {
                foreach (var itemExpected in productExpected.ProductItems)
                {
                    if (item.Id == itemExpected.Id)
                    {
                        Assert.Equal(item.Id, itemExpected.Id);
                        Assert.Equal(item.Modelo, itemExpected.Modelo);
                        Assert.Equal(item.Value, itemExpected.Value);
                        Assert.Equal(item.IsSold, itemExpected.IsSold);
                        Assert.Equal(item.IsAvailable, itemExpected.IsAvailable);
                        Assert.Equal(item.ImageZip, itemExpected.ImageZip);
                    }
                }
            }
        }

        #endregion Test validate expected data
    }
}