using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services;
using SystemV1.Domain.Services.Notifications;
using SystemV1.Domain.Services.Validations;
using SystemV1.Domain.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.ServiceTests
{
    [Collection(nameof(ProductCollection))]
    public class ProductServiceTest : ServiceTestBase
    {
        private readonly ProductTestFixture _productTestFixture;
        private readonly ProductItemTestFixture _productItemTestFixture;

        public ProductServiceTest(ProductTestFixture productTestFixture) : base(new AutoMocker())
        {
            _productTestFixture = productTestFixture;
            _productItemTestFixture = new ProductItemTestFixture();
        }

        #region Function Add

        [Fact(DisplayName = "Add only new valid product")]
        [Trait("UnitTests - Services", "Product")]
        public async Task Add_AddOnlyNewValidProduct_ShouldBeFailed()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct(Guid.NewGuid(), false);

            SetSetupMock<Product, IValidationProduct>(product, GenerateMockErrors("Name", ValidationProduct.NameRequired));
            var productService = _autoMocker.CreateInstance<ServiceProduct>();

            //Act
            await productService.AddAsyncUow(product);

            //Assert
            _autoMocker.GetMock<IRepositoryProduct>()
                  .Verify(r => r.Add(It.IsAny<Product>()), Times.Never);
            _autoMocker.GetMock<IServiceProductItem>()
                  .Verify(s => s.AddAsyncUow(It.IsAny<ProductItem>()), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>()
                  .Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Add new valid product with valid produtcItems")]
        [Trait("UnitTests - Services", "Product")]
        public async Task Add_AddNewValidProductWithValidProductItems_ShouldAddProductAndProductItemsWithSuccess()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct(Guid.NewGuid());
            SetSetupMock<Product, IValidationProduct>(product);
            var productService = _autoMocker.CreateInstance<ServiceProduct>();

            //Act
            await productService.AddAsyncUow(product);

            //Assert
            _autoMocker.GetMock<IRepositoryProduct>()
                  .Verify(r => r.Add(It.IsAny<Product>()), Times.Once);
            _autoMocker.GetMock<IServiceProductItem>()
                  .Verify(s => s.AddAsyncUow(It.IsAny<ProductItem>()), Times.Exactly(product.ProductItems.Count));
            _autoMocker.GetMock<IUnitOfWork>()
                  .Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add only new invalid product")]
        [Trait("UnitTests - Services", "Product")]
        public async Task Add_AddOnlyInvalidProduct_ShouldNotAddProduct()
        {
            //Arrange
            var product = _productTestFixture.GenerateInvalidProduct();
            SetSetupMock<Product, IValidationProduct>(product, GenerateMockErrors("Name", ValidationProduct.NameRequired));
            var productService = _autoMocker.CreateInstance<ServiceProduct>();

            //Act
            await productService.AddAsyncUow(product);

            //Assert
            _autoMocker.GetMock<IRepositoryProduct>()
                 .Verify(r => r.Add(It.IsAny<Product>()), Times.Never);
            _autoMocker.GetMock<IServiceProductItem>()
                  .Verify(s => s.AddAsyncUow(It.IsAny<ProductItem>()), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>()
                  .Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Add valid product and invalid product items")]
        [Trait("UnitTests - Services", "Product")]
        public async Task Add_AddValidProductAndInvalidProdutcItems_ShoudNotAddProductAndProductItem()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct(Guid.NewGuid(), true, false);
            SetSetupMock<Product, IValidationProduct>(product);
            SetSetupMock<ProductItem, ValidationProductItem>(product.ProductItems.FirstOrDefault(),
                                                             GenerateMockErrors("Modelo", ValidationProductItem.ProductItemRequired));
            var productService = _autoMocker.CreateInstance<ServiceProduct>();

            //Act
            await productService.AddAsyncUow(product);

            //Assert
            _autoMocker.GetMock<IServiceProductItem>()
                  .Verify(s => s.AddAsyncUow(It.IsAny<ProductItem>()), Times.Exactly(product.ProductItems.Count()));

            _autoMocker.GetMock<IRepositoryProduct>()
                  .Verify(r => r.Add(It.IsAny<Product>()), Times.Once);

            _autoMocker.GetMock<IUnitOfWork>()
                  .Verify(r => r.CommitAsync(), Times.Never);
        }

        #endregion Function Add

        #region Function Update

        [Fact(DisplayName = "Update valid product with success")]
        [Trait("UnitTests - Services", "Product")]
        public async Task Update_UpdateValidProductAndProductItems_ShouldUpdateWithSuccess()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct(Guid.NewGuid());
            SetSetupMock<Product, IValidationProduct>(product);
            var serviceProduct = _autoMocker.CreateInstance<ServiceProduct>();

            //Act
            await serviceProduct.UpdateAsyncUow(product);

            //Assert
            _autoMocker.GetMock<IRepositoryProduct>()
                  .Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>()
                  .Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update product without product items")]
        [Trait("UnitTests - Services", "Product")]
        public async Task Update_UpdateValidProductWithoutProductItems_ShouldNotUpdate()
        {
            //Assert
            var product = _productTestFixture.GenerateValidProduct(Guid.NewGuid(), false);
            SetSetupMock<Product, IValidationProduct>(product, GenerateMockErrors("ProductItems", ValidationProduct.ProductItemNotEmpyt));
            var serviceProduct = _autoMocker.CreateInstance<ServiceProduct>();

            //Act
            await serviceProduct.UpdateAsyncUow(product);

            //Assert
            _autoMocker.GetMock<IRepositoryProduct>()
                  .Verify(r => r.Update(It.IsAny<Product>()), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>()
                  .Verify(u => u.CommitAsync(), Times.Never);
            _autoMocker.GetMock<INotifier>()
                  .Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Update valid product with invalid product item")]
        [Trait("UnitTests - Services", "Product")]
        public async Task Update_UpdateValidProductWithInvalidProductItem_ShouldNotUpdateProductAndProductItem()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct(Guid.NewGuid(), true, false);

            var serviceProduct = _autoMocker.CreateInstance<ServiceProduct>();

            //Act
            await serviceProduct.UpdateAsyncUow(product);

            //Assert
            _autoMocker.GetMock<IRepositoryProduct>().Verify(r => r.Update(It.IsAny<Product>()), Times.Never);
            _autoMocker.GetMock<IRepositoryProductItem>().Verify(r => r.Update(It.IsAny<ProductItem>()), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }

        #endregion Function Update

        #region Function Get

        [Fact(DisplayName = "Get all products with success")]
        [Trait("UnitTests - Services", "Product")]
        public async Task GetAll_GetAllProducts_ShouldReturnAllProducts()
        {
            //Arrange
            var page = 1;
            var pageSize = 10;
            _autoMocker.GetMock<IRepositoryProduct>()
                  .Setup(r => r.SearchAsync(null, page, pageSize))
                  .Returns(Task.FromResult(_productTestFixture.GenerateProduct(10, Guid.NewGuid())));
            var serviceProduct = _autoMocker.CreateInstance<ServiceProduct>();

            //Act
            var products = await serviceProduct.SearchAsync(null, page, pageSize);

            //Assert
            _autoMocker.GetMock<IRepositoryProduct>().Verify(r => r.SearchAsync(null, page, pageSize), Times.Once);
            products.Should().NotBeNull();
            products.Should().HaveCount(10);
        }

        [Fact(DisplayName = "Get product by id with success")]
        [Trait("UnitTests - Services", "Product")]
        public async Task GetById_GetProductById_ShouldReturnProduct()
        {
            //Arrange

            var productId = Guid.NewGuid();
            _autoMocker.GetMock<IRepositoryProduct>()
                  .Setup(r => r.GetEntityAsync(p => p.Id == productId, ""))
                  .Returns(Task.FromResult(_productTestFixture.GenerateValidProduct(Guid.NewGuid())));
            var serviceProduct = _autoMocker.CreateInstance<ServiceProduct>();

            //Act
            var product = await serviceProduct.GetEntityAsync(p => p.Id == productId, "");

            //Assert
            _autoMocker.GetMock<IRepositoryProduct>().Verify(r => r.GetEntityAsync(p => p.Id == productId, ""), Times.Once);
            product.Should().NotBeNull();
        }

        #endregion Function Get

        #region Function Remove

        [Fact(DisplayName = "Remove product with success")]
        [Trait("UnitTests - Services", "Product")]
        public async Task Remove_RemoveProduct_ShouldRemoveWithSuccess()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct(Guid.NewGuid());
            SetSetupMock<Product, IValidationProduct>(product);
            var serviceProduct = _autoMocker.CreateInstance<ServiceProduct>();

            //Act
            await serviceProduct.RemoveAsyncUow(product);

            //Assert
            _autoMocker.GetMock<IRepositoryProduct>().Verify(r => r.Remove(product), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        #endregion Function Remove
    }
}