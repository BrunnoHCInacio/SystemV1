using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services;
using SystemV1.Domain.Services.Validations;
using SystemV1.Domain.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.ServiceTests
{
    [Collection(nameof(ProductItemCollection))]
    public class ProductItemServiceTest : ServiceTestBase
    {
        private readonly ProductItemTestFixture _productItemTestFixture;

        public ProductItemServiceTest(ProductItemTestFixture productItemTestFixture) : base(new AutoMocker())
        {
            _productItemTestFixture = productItemTestFixture;
        }

        [Fact(DisplayName = "Add new valid product item")]
        [Trait("UnitTests - Services", "Product Item")]
        public async Task ProductItem_AddNewValidProductItem_ShouldAddWithSucess()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var productItem = _productItemTestFixture.GenerateValidProductItem(productId, providerId);
            SetSetupMock<ProductItem, IValidationProductItem>(productItem);
            var service = _autoMocker.CreateInstance<ServiceProductItem>();

            //Act
            await service.AddAsyncUow(productItem);

            //Assert
            _autoMocker.GetMock<IRepositoryProductItem>().Verify(r => r.Add(productItem), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add new invalid product item")]
        [Trait("UnitTests - Services", "Product Item")]
        public async Task ProductItem_AddNewInvalidProductItem_ShouldNotAdd()
        {
            //Arrange
            var productItem = _productItemTestFixture.GenerateInvalidProductItem();
            SetSetupMock<ProductItem, IValidationProductItem>(productItem, GenerateMockErrors("Modelo", ValidationProductItem.ProductItemRequired));
            var service = _autoMocker.CreateInstance<ServiceProductItem>();

            //Act
            await service.AddAsyncUow(productItem);

            //Assert
            _autoMocker.GetMock<IRepositoryProductItem>().Verify(r => r.Add(productItem), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Update valid product item")]
        [Trait("UnitTests - Services", "Product Item")]
        public async Task ProductItem_UpdateValidProductItem_ShouldUpdateWithSucess()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var productItem = _productItemTestFixture.GenerateValidProductItem(productId, providerId);
            SetSetupMock<ProductItem, IValidationProductItem>(productItem);
            var service = _autoMocker.CreateInstance<ServiceProductItem>();

            //Act
            await service.UpdateAsyncUow(productItem);

            //Assert
            _autoMocker.GetMock<IRepositoryProductItem>().Verify(r => r.Update(productItem), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update invalid product item")]
        [Trait("UnitTests - Services", "Product Item")]
        public async Task ProductItem_UpdateInvalidProductItem_ShouldNotUpdate()
        {
            //Arrange
            var productItem = _productItemTestFixture.GenerateInvalidProductItem();
            SetSetupMock<ProductItem, IValidationProductItem>(productItem, GenerateMockErrors("Modelo", ValidationProductItem.ProductItemRequired));
            var service = _autoMocker.CreateInstance<ServiceProductItem>();

            //Act
            await service.UpdateAsyncUow(productItem);

            //Assert
            _autoMocker.GetMock<IRepositoryProductItem>().Verify(r => r.Update(productItem), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Delete valid product item")]
        [Trait("UnitTests - Services", "Product Item")]
        public async Task ProductItem_DeleteValidProductItem_ShouldDeleteWithSucess()
        {
            //Arrange
            var productId = Guid.NewGuid();
            var providerId = Guid.NewGuid();
            var productItem = _productItemTestFixture.GenerateValidProductItem(productId, providerId);
            SetSetupMock<ProductItem, IValidationProductItem>(productItem);
            var service = _autoMocker.CreateInstance<ServiceProductItem>();

            //Act
            await service.RemoveAsyncUow(productItem);

            //Assert
            _autoMocker.GetMock<IRepositoryProductItem>().Verify(r => r.Remove(productItem), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Get all product items with paginate")]
        [Trait("UnitTests - Services", "Product Item")]
        public async Task ProductItem_GetAllPaginateProductItems_ShouldReturnListOfProductItems()
        {
            //Arrange
            var page = 1;
            var pageSize = 10;
            _autoMocker.GetMock<IRepositoryProductItem>()
                       .Setup(r => r.SearchAsync(null, page, pageSize))
                       .Returns(Task.FromResult(_productItemTestFixture.GenerateProductItem(pageSize,
                                                                                            Guid.NewGuid(),
                                                                                            Guid.NewGuid())));
            var service = _autoMocker.CreateInstance<ServiceProductItem>();

            //Act
            var productItens = await service.SearchAsync(null, page, pageSize);

            //Assert
            _autoMocker.GetMock<IRepositoryProductItem>().Verify(r => r.SearchAsync(null, page, pageSize), Times.Once);
            productItens.Should().NotBeNull();
            productItens.Should().HaveCount(pageSize);
        }

        [Fact(DisplayName = "Get product item by id")]
        [Trait("UnitTests - Services", "Product Item")]
        public async Task ProductItem_GetProductItemById_ShouldReturnProductItem()
        {
            //Arrange
            var productId = Guid.NewGuid();
            _autoMocker.GetMock<IRepositoryProductItem>()
                       .Setup(r => r.GetEntityAsync(p => p.Id == productId, ""))
                       .Returns(Task.FromResult(_productItemTestFixture.GenerateValidProductItem(Guid.NewGuid(),
                                                                                                 Guid.NewGuid())));
            var service = _autoMocker.CreateInstance<ServiceProductItem>();

            //Act
            var productIten = await service.GetEntityAsync(p => p.Id == productId, "");

            //Assert
            _autoMocker.GetMock<IRepositoryProductItem>().Verify(r => r.GetEntityAsync(p => p.Id == productId, ""), Times.Once);
            productIten.Should().NotBeNull();
        }
    }
}