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
    [Collection(nameof(ProductItemCollection))]
    public class ProductItemDomainTest
    {
        private readonly ProductItemTestFixture _productItemTestFixture;

        public ProductItemDomainTest(ProductItemTestFixture productItemTestFixture)
        {
            _productItemTestFixture = productItemTestFixture;
        }

        [Fact(DisplayName = "Validate as correct set properties on the product item ")]
        [Trait("Categoria", "Cadastro - Product Item")]
        public void ProductItem_NewProduct_ShouldSetCorrectProperties()
        {
            //Arrange
            var productItemExpected = _productItemTestFixture.GenerateProductExpected();

            //Act
            var productItem = new ProductItem(productItemExpected.Id,
                                              productItemExpected.Modelo,
                                              productItemExpected.Value,
                                              productItemExpected.ImageZip);

            //Assert
            Assert.Equal(productItemExpected.Id, productItem.Id);
            Assert.Equal(productItemExpected.Modelo, productItem.Modelo);
            Assert.Equal(productItemExpected.Value, productItem.Value);
            Assert.Equal(productItemExpected.ImageZip, productItem.ImageZip);
        }

        //TODO: Criar testes para validar a view model.

        [Fact(DisplayName = "Validate valid product item ")]
        [Trait("Categoria", "Cadastro - Product Item")]
        public void ProductItem_NewProduct_ShouldBeValid()
        {
            //Arrange
            var productItem = _productItemTestFixture.GenerateValidProduct();

            //Act
            var result = productItem.ValidateProductItem();

            //Assert
            Assert.True(result.IsValid);
        }

        //TODO: Criar testes para validar objeto desabilitado.

        [Fact(DisplayName = "Validate invalid product item ")]
        [Trait("Categoria", "Cadastro - Product Item")]
        public void ProductItem_NewProduct_ShouldBeInvalid()
        {
            //Arrange
            var productItem = _productItemTestFixture.GenerateInvalidProduct();

            //Act
            var result = productItem.ValidateProductItem();

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any());

            Assert.Single(result.Errors);
            Assert.Contains(ProductItemValidation.ProductItemRequired, result.Errors.Select(e => e.ErrorMessage));
        }
    }
}