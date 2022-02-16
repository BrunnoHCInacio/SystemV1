using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services;
using SystemV1.Domain.Services.Notifications;
using SystemV1.Domain.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.ServiceTests
{
    [Collection(nameof(ProductCollection))]
    public class ProductServiceTest
    {
        private readonly ProductTestFixture _productTestFixture;
        private readonly ProductItemTestFixture _productItemTestFixture;

        public ProductServiceTest(ProductTestFixture productTestFixture)
        {
            _productTestFixture = productTestFixture;
            _productItemTestFixture = new ProductItemTestFixture();
        }

        #region Function Add
        [Fact(DisplayName ="Add only new valid product")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task Add_AddOnlyNewValidProduct_ShouldNotAddWithSuccess()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct(false);
            var mocker = new AutoMocker();
            var productService = mocker.CreateInstance<ServiceProduct>();

            //Act
            await productService.AddAsyncUow(product);

            //Assert
            mocker.GetMock<IRepositoryProduct>()
                  .Verify(r => r.Add(It.IsAny<Product>()), Times.Never);
            mocker.GetMock<IRepositoryProductItem>()
                  .Verify(s => s.Add(It.IsAny<ProductItem>()), Times.Never);
            mocker.GetMock<IUnitOfWork>()
                  .Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Add new valid product with valid produtcItems")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task Add_AddNewValidProductWithValidProductItems_ShouldAddProductAndProductItemsWithSuccess()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct();
            var mocker = new AutoMocker();
            var productService = mocker.CreateInstance<ServiceProduct>();

            //Act
            await productService.AddAsyncUow(product);

            //Assert
            mocker.GetMock<IRepositoryProduct>()
                  .Verify(r => r.Add(It.IsAny<Product>()), Times.Once);
            mocker.GetMock<IRepositoryProductItem>()
                  .Verify(s => s.Add(It.IsAny<ProductItem>()), Times.Exactly(product.ProductItems.Count));
            mocker.GetMock<IUnitOfWork>()
                  .Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName ="Add only new invalid product")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task Add_AddOnlyInvalidProduct_ShouldNotAddProduct()
        {
            //Arrange
            var product = _productTestFixture.GenerateInvalidProduct();
            var mocker = new AutoMocker();
            var productService = mocker.CreateInstance<ServiceProduct>();

            //Act
            await productService.AddAsyncUow(product);

            //Assert
            mocker.GetMock<IRepositoryProduct>()
                 .Verify(r => r.Add(It.IsAny<Product>()), Times.Never);
            mocker.GetMock<IRepositoryProductItem>()
                  .Verify(s => s.Add(It.IsAny<ProductItem>()), Times.Never);
            mocker.GetMock<IUnitOfWork>()
                  .Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName ="Add valid product and invalid product items")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task Add_AddValidProductAndInvalidProdutcItems_ShoudNotAddProductAndProductItem()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct(true, false);
            var mocker = new AutoMocker();
            var productService = mocker.CreateInstance<ServiceProduct>();

            //Act
            await productService.AddAsyncUow(product);

            //Assert
            mocker.GetMock<IRepositoryProduct>()
                  .Verify(r => r.Add(It.IsAny<Product>()), Times.Never);
            mocker.GetMock<IRepositoryProductItem>()
                  .Verify(s => s.Add(It.IsAny<ProductItem>()), Times.Never);
            mocker.GetMock<IUnitOfWork>()
                  .Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName ="Add valid product with exception on product repository")]
        [Trait("Categoria","Produto - Serviço")]
        public async Task Add_AddValidProductWithException_ShouldNotAddProduct()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProduct>()
                  .Setup(r => r.Add(It.IsAny<Product>()))
                  .Throws(new Exception());
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            await serviceProduct.AddAsyncUow(product);

            //Assert
            mocker.GetMock<IRepositoryProduct>()
                  .Verify(r => r.Add(It.IsAny<Product>()), Times.Once);
            mocker.GetMock<IRepositoryProductItem>()
                  .Verify(s => s.Add(It.IsAny<ProductItem>()), Times.Exactly(product.ProductItems.Count));
            mocker.GetMock<IUnitOfWork>()
                  .Verify(r => r.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>()
                  .Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            Assert.Throws<Exception>(()=>serviceProduct.Add(product));
        }

        [Fact(DisplayName ="Add new valid product and valid product items with exception on repository product item")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task Add_AddNewValidProductAndValidProductItemsWithExceptionOnRepositoryProductItem_ShouldNotAddProducAndProductItem()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProductItem>()
                  .Setup(r => r.Add(It.IsAny<ProductItem>()))
                  .Throws(new Exception());
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            await serviceProduct.AddAsyncUow(product);

            //Assert
            mocker.GetMock<IRepositoryProduct>()
                 .Verify(r => r.Add(It.IsAny<Product>()), Times.Never);
            mocker.GetMock<IRepositoryProductItem>()
                  .Verify(s => s.Add(It.IsAny<ProductItem>()), Times.Once);
            mocker.GetMock<IUnitOfWork>()
                  .Verify(r => r.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>()
                  .Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            Assert.Throws<Exception>(() => serviceProduct.Add(product));
        }
        #endregion

        #region Function Update

        [Fact(DisplayName = "Update valid product with success")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task Update_UpdateValidProductAndProductItems_ShouldUpdateWithSuccess()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct();
            var mocker = new AutoMocker();
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            await serviceProduct.UpdateAsyncUow(product);

            //Assert
            mocker.GetMock<IRepositoryProduct>()
                  .Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
            mocker.GetMock<IUnitOfWork>()
                  .Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update product without product items")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task Update_UpdateValidProductWithoutProductItems_ShouldNotUpdate()
        {
            //Assert
            var product = _productTestFixture.GenerateValidProduct(false);
            var mocker = new AutoMocker();
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            await serviceProduct.UpdateAsyncUow(product);

            //Assert
            mocker.GetMock<IRepositoryProduct>()
                  .Verify(r => r.Update(It.IsAny<Product>()), Times.Never);
            mocker.GetMock<IUnitOfWork>()
                  .Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>()
                  .Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName ="Update valid product with invalid product item")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task Update_UpdateValidProductWithInvalidProductItem_ShouldNotUpdateProductAndProductItem()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct(true, false);
            var mocker = new AutoMocker();
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            await serviceProduct.UpdateAsyncUow(product);

            //Assert
            mocker.GetMock<IRepositoryProduct>().Verify(r => r.Update(It.IsAny<Product>()), Times.Never);
            mocker.GetMock<IRepositoryProductItem>().Verify(r => r.Update(It.IsAny<ProductItem>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);

        }

        [Fact(DisplayName ="Update valid product with exception on repository product")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task Update_UpdateValidProductWithExceptionOnProductRepository_ShouldNotUpdate()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProduct>().Setup(r => r.Update(It.IsAny<Product>())).Throws(new Exception());
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            await serviceProduct.UpdateAsyncUow(product);

            //Assert
            mocker.GetMock<IRepositoryProduct>().Verify(r => r.Update(It.IsAny<Product>()), Times.Once);
            mocker.GetMock<IRepositoryProductItem>().Verify(r => r.Update(It.IsAny<ProductItem>()), Times.Never);
            mocker.GetMock<INotifier>().Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
            Assert.Throws<Exception>(() => serviceProduct.Update(product));
        }



        #endregion

        #region Function Get
        
        [Fact(DisplayName ="Get all products with success")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task GetAll_GetAllProducts_ShouldReturnAllProducts()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProduct>()
                  .Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                  .Returns(Task.FromResult((IEnumerable<Product>)_productTestFixture.GenerateProduct(10)));
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            var products = await serviceProduct.GetAllAsync(1, 1);

            //Assert
            mocker.GetMock<IRepositoryProduct>().Verify(r => r.GetAllAsync(1, 1), Times.Once);
            products.Should().NotBeNull();
            products.Should().HaveCount(10);
        }

        [Fact(DisplayName = "Get all products with exception")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task GetAll_GetAllProductsWithException_ShouldNotReturnAllProducts()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProduct>()
                  .Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                  .Throws(new Exception());
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            var products = await serviceProduct.GetAllAsync(1, 1);

            //Assert
            mocker.GetMock<IRepositoryProduct>().Verify(r => r.GetAllAsync(1, 1), Times.Once);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            products.Should().BeNull();
            
        }

        [Fact(DisplayName = "Get product by id with success")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task GetById_GetProductById_ShouldReturnProduct()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProduct>()
                  .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .Returns(Task.FromResult(_productTestFixture.GenerateValidProduct()));
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            var product = await serviceProduct.GetByIdAsync(Guid.NewGuid());

            //Assert
            mocker.GetMock<IRepositoryProduct>().Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            product.Should().NotBeNull();
        }

        [Fact(DisplayName = "Get product by id with exception")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task GetById_GetProductByIdWithException_ShouldNotReturnProduct()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProduct>()
                  .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .Throws(new Exception());
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            var product = await serviceProduct.GetByIdAsync(Guid.NewGuid());

            //Assert
            mocker.GetMock<IRepositoryProduct>().Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<INotifier>().Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
            product.Should().BeNull();
        }

        [Fact(DisplayName = "Get product by name with success")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task GetById_GetProductByName_ShouldReturnProduct()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProduct>()
                  .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                  .Returns(Task.FromResult((IEnumerable<Product>)_productTestFixture.GenerateProduct(5)));
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            var products = await serviceProduct.GetByNameAsync("AAaaa");

            //Assert
            mocker.GetMock<IRepositoryProduct>().Verify(r => r.GetByNameAsync(It.IsAny<string>()), Times.Once);
            products.Should().NotBeNull();
            products.Should().HaveCount(5);
        }

        [Fact(DisplayName = "Get product by invalid name ")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task GetById_GetProductByInvalidName_ShouldNotReturnProduct()
        {
            //Arrange
            var mocker = new AutoMocker();
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            var products = await serviceProduct.GetByNameAsync("");

            //Assert
            mocker.GetMock<IRepositoryProduct>().Verify(r => r.GetByNameAsync(It.IsAny<string>()), Times.Never);
            products.Should().BeNull();
        }

        [Fact(DisplayName ="Get product by id with exception")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task GetById_GetProductByValidNameWithException_ShouldNotReturnProduct()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProduct>()
                  .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                  .Throws(new Exception());
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            var products = await serviceProduct.GetByNameAsync("AAaaa");

            //Assert
            mocker.GetMock<IRepositoryProduct>().Verify(r => r.GetByNameAsync(It.IsAny<string>()), Times.Once);
            mocker.GetMock<INotifier>().Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
            products.Should().BeNull();
            
        }

        [Fact(DisplayName ="Get all product items with success")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task GetAllProductItems_GetAllProductItems_ShouldReturnWithSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProductItem>()
                  .Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                  .Returns(Task.FromResult((IEnumerable <ProductItem>) _productItemTestFixture.GenerateProductItem(10)));
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            var product = await serviceProduct.GetAllProductItemAsync(1, 1);

            //Assert 
            product.Should().NotBeNull();
            mocker.GetMock<IRepositoryProductItem>().Verify(r => r.GetAllAsync(1, 1), Times.Once);
        }

        [Fact(DisplayName ="Get all product items with exception")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task GetAllProductItems_GetAllProductItemsWithExceptionOnRepositoryProductItem_ShouldNotReturnAnsThrowException()
        {
            //Assert
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProductItem>()
                  .Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                  .Throws(new Exception());
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            var productItems = await serviceProduct.GetAllProductItemAsync(1, 1);

            //Assert
            productItems.Should().BeNull();
            mocker.GetMock<IRepositoryProductItem>().Verify(r => r.GetAllAsync(1, 1), Times.Once);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Get product item by id with success")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task GetProductItemById_GetProductItemById_ShoudReturnWithSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProductItem>()
                  .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .Returns(Task.FromResult(_productItemTestFixture.GenerateValidProduct()));
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            var productItem = await serviceProduct.GetProductItemByIdAsync(Guid.NewGuid());

            //Assert
            productItem.Should().NotBeNull();
            mocker.GetMock<IRepositoryProductItem>().Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact(DisplayName ="Get product item by id with exception")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task GetProductItemById_GetProductItemByIdWithExceptionOnRepositoryProductItem_ShoudNotReturnAndThrowException()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProductItem>()
                  .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .Throws(new Exception());
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            var productItem = await serviceProduct.GetProductItemByIdAsync(Guid.NewGuid());

            //Assert
            productItem.Should().BeNull();
            mocker.GetMock<IRepositoryProductItem>().Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<INotifier>().Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName ="Get product item by name with success")]
        [Trait("Categoria","Produto - Serviço")]
        public async Task GetProductItemByName_GetProductItemByValidName_ShoudReturnWithSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProductItem>()
                  .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                  .Returns(Task.FromResult((IEnumerable<ProductItem>)_productItemTestFixture.GenerateProductItem(5)));
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            var productItem = await serviceProduct.GetProductItemByNameAsync("Addasdf");

            //Assert
            productItem.Should().NotBeNull();
            mocker.GetMock<IRepositoryProductItem>().Verify(r => r.GetByNameAsync(It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName ="Get product item by name with invalid name")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task GetProductItemByName_GetProductItemByInvalidName_ShoudNotReturn()
        {
            //Arrange
            var mocker = new AutoMocker();
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            var productItem = await serviceProduct.GetProductItemByNameAsync("");

            //Assert
            productItem.Should().BeNull();
            mocker.GetMock<IRepositoryProductItem>().Verify(r => r.GetByNameAsync(It.IsAny<string>()), Times.Never);
        }

        [Fact(DisplayName ="Get product item by name with exception")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task GetProductItemByName_GetProductItemByValidName_ShoudNotReturnAndthrowException()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProductItem>()
                  .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                  .Throws(new Exception());
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            var productItem = await serviceProduct.GetProductItemByNameAsync("Addasdf");

            //Assert
            productItem.Should().BeNull();
            mocker.GetMock<IRepositoryProductItem>().Verify(r => r.GetByNameAsync(It.IsAny<string>()), Times.Once);
            mocker.GetMock<INotifier>().Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
            
        }
        #endregion

        #region Function Remove
        [Fact(DisplayName ="Remove product with success")]
        [Trait("Categoria","Produto - Serviço")]
        public async Task Remove_RemoveProduct_ShouldRemoveWithSuccess()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct();
            var mocker = new AutoMocker();
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            await serviceProduct.RemoveAsyncUow(product);

            //Assert
            mocker.GetMock<IRepositoryProduct>().Verify(r => r.Update(product), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);

        }

        [Fact(DisplayName ="Remove product with exception on product repository")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task Remove_RemoveProductWithExceptionOnProductRepository_ShouldNotRemoveProduct()
        {
            //Arrange
            var product = _productTestFixture.GenerateValidProduct();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProduct>().Setup(r => r.Update(It.IsAny<Product>())).Throws(new Exception());
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            await serviceProduct.RemoveAsyncUow(product);

            //Assert 
            mocker.GetMock<IRepositoryProduct>().Verify(n => n.Update(It.IsAny<Product>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(n => n.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            Assert.Throws<Exception>(() => serviceProduct.Remove(product));
            
        }

        [Fact(DisplayName ="Remove product item with sucess")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task RemoveProductItem_RemoveProductItem_ShouldRemoveWithSuccess() 
        {
            //Arrange
            var productItem = _productItemTestFixture.GenerateValidProduct();
            var mocker = new AutoMocker();
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            await serviceProduct.RemoveProductItemAsyncUow(productItem);

            //Assert
            mocker.GetMock<IRepositoryProductItem>()
                  .Verify(r => r.Update(It.IsAny<ProductItem>()), Times.Once);
            mocker.GetMock<IUnitOfWork>()
                  .Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName ="Remove product item with exception on repository product item")]
        [Trait("Categoria", "Produto - Serviço")]
        public async Task RemoveProductItem_RemoveProductItemWithExceptionOnRepositoryProductItem_ShouldNotRemoveAndThrowException()
        {
            //Arrange
            var productItem = _productItemTestFixture.GenerateValidProduct();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProductItem>()
                  .Setup(r => r.Update(It.IsAny<ProductItem>()))
                  .Throws(new Exception());
            var serviceProduct = mocker.CreateInstance<ServiceProduct>();

            //Act
            await serviceProduct.RemoveProductItemAsyncUow(productItem);

            //Assert
            mocker.GetMock<IRepositoryProductItem>()
                  .Verify(r => r.Update(It.IsAny<ProductItem>()), Times.Once);
            mocker.GetMock<IUnitOfWork>()
                  .Verify(r => r.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>()
                  .Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
            Assert.Throws<Exception>(() => serviceProduct.RemoveProductItem(productItem));
        }
        #endregion
    }
}
