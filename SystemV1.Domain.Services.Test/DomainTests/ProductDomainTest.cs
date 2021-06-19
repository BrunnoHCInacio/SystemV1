using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Test.Fixture;
using SystemV1.Domain.Validations;
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

        [Fact(DisplayName = "Validate set as correct properties on the product")]
        [Trait("Categoria", "Cadastro - Produto")]
        public void Product_NewProduct_ShouldSetCorrectProperties()
        {
            //Arrange
            var productExpected = _productTestFixture.GenerateProductExpected();

            //Act
            var product = new Product(productExpected.Id, productExpected.Name);

            //Assert
            Assert.Equal(productExpected.Id, product.Id);
            Assert.Equal(productExpected.Name, productExpected.Name);
        }

        [Fact(DisplayName = "Validate valid product")]
        [Trait("Categoria", "Cadastro - Produto")]
        public void Product_NewProduct_ShouldBeValid()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct();

            //Act
            var result = product.ValidateProduct();

            //Assert
            Assert.True(result.IsValid);
            Assert.True(product.ProductItems.Any());
            Assert.Equal(3, product.ProductItems.Count());
        }

        [Fact(DisplayName = "Validate invalid product")]
        [Trait("Categoria", "Cadastro - Produto")]
        public void Product_NewProduct_ShouldBeInvalid()
        {
            //Arrange
            var product = _productTestFixture.GenerateInvalidProduct();

            //Act
            var result = product.ValidateProduct();

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any());

            Assert.Contains(ProductValidation.NameRequired, result.Errors.Select(e => e.ErrorMessage));
        }
    }
}