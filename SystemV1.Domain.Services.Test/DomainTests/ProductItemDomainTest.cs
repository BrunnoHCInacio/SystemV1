using System;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.DomainTests
{
    [Collection(nameof(ProductItemCollection))]
    public class ProductItemDomainTest
    {
        private readonly ProductItemTestFixture _productItemTestFixture;

        public ProductItemDomainTest(ProductItemTestFixture productItemTestFixture)
        {
            _productItemTestFixture = productItemTestFixture;
        }

        [Fact(DisplayName = "Validate as correct set properties on the product item ")]
        [Trait("UnitTests - Entity", "Product Item")]
        public void ProductItem_NewProduct_ShouldSetCorrectProperties()
        {
            //Arrange

            var productItemExpected = _productItemTestFixture.GenerateProductItemExpected();

            var productFixture = new ProductTestFixture();
            var product = productFixture.GenerateProductExpected();

            //Act
            var productItem = new ProductItem(productItemExpected.Id,
                                              productItemExpected.Modelo,
                                              productItemExpected.Value,
                                              new Product(product.Id, product.Name, product.ProviderId),
                                              productItemExpected.ImageZip);

            //Assert
            Assert.Equal(productItemExpected.Id, productItem.Id);
            Assert.Equal(productItemExpected.Modelo, productItem.Modelo);
            Assert.Equal(productItemExpected.Value, productItem.Value);
            Assert.Equal(productItemExpected.ImageZip, productItem.ImageZip);
            Assert.NotNull(productItem.Product);
            Assert.NotEqual(Guid.Empty, productItem.Product.Id);
        }
    }
}