using FluentValidation.Results;
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
        #region Test validate expected data

        [Fact(DisplayName = "Validate set as correct properties for product")]
        [Trait("Categoria", "Produto - Cadastro")]
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

        [Fact(DisplayName = "Validate set as correct properties for product item")]
        [Trait("Categoria","Produto - Cadastro")]
        public void Product_NewProductWithProductItem_ShouldSetCorrectProperties()
        {
            //Arrange
            var productExpected = _productTestFixture.GenerateProductExpected();
           
            //Act
            var product = new Product(productExpected.Id, productExpected.Name);

            foreach (var item in productExpected.ProductItems)
            {
                var productItem = new ProductItem(item.Id, 
                                                  item.Modelo, 
                                                  item.Value, 
                                                  item.ImageZip);
                productItem.SetProductForSale();
                
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
                        Assert.Equal(item.Id,itemExpected.Id);
                        Assert.Equal(item.Modelo, itemExpected.Modelo);
                        Assert.Equal(item.Value, itemExpected.Value);
                        Assert.Equal(item.IsSold, itemExpected.IsSold);
                        Assert.Equal(item.IsAvailable, itemExpected.IsAvailable);
                        Assert.Equal(item.ImageZip, itemExpected.ImageZip);
                    }
                }
                
            }
        }
        #endregion
        

        [Fact(DisplayName = "Validate valid product")]
        [Trait("Categoria", "Produto - Cadastro")]
        public void Product_NewProduct_ShouldBeValid()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct();

            //Act
            var result = product.ValidateProduct();

            //Assert
            Assert.True(result.IsValid);
        }

        //TODO: Criar teste para validar as propriedades de produto e produto com item(s).
        [Fact(DisplayName = "Validate valid product with produtc items")]
        [Trait("Categoria", "Produto - Cadastro")]
        public void Product_NewProductWithProductItems_ShouldBeValid()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct();

            //Act
            var result = product.ValidateProduct();
            var resultProductItems = new List<ValidationResult>();

            //Assert
            Assert.True(result.IsValid);
            
            foreach (var resultItem in resultProductItems)
            {
                Assert.True(resultItem.IsValid);
            }
        }

        [Fact(DisplayName = "Validate invalid product")]
        [Trait("Categoria", "Produto - Cadastro")]
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

        //TODO: Criar teste de validação para produto desabilitado.
        [Fact(DisplayName = "Validate valid product")]
        [Trait("Categoria", "Produto - Cadastro")]
        public void Product_NewProductDisabled_ShouldFailed()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProductDisabled();

            //Act
            var result = product.ValidateProduct();

            //Assert
            Assert.False(result.IsValid);
            Assert.Single(result.Errors);
            Assert.Contains(ProductValidation.ProductNotActive, result.Errors.Select(e=>e.ErrorMessage));
        }

    }
}