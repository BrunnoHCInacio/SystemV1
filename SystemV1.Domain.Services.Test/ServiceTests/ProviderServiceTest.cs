﻿using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
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
    [Collection(nameof(ProviderCollection))]
    public  class ProviderServiceTest
    {
        private readonly ProviderTestFixture _providerTestFixture;

        public ProviderServiceTest(ProviderTestFixture providerTestFixture)
        {
            _providerTestFixture = providerTestFixture;
        }

        #region Function Add

        [Fact(DisplayName ="Add new provider with success")]
        [Trait("Categoria", "Fornecedor - Serviço")]
        public async Task Add_AddNewProviderValid_ShouldAddWithSuccess()
        {
            //Arrange
            var provider = _providerTestFixture.GenerateValidProvider();
            var mocker = new AutoMocker();
            var serviceProvider = mocker.CreateInstance<ProviderService>();

            //Act
            await serviceProvider.AddAsyncUow(provider);

            //Assert
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.Add(It.IsAny<Provider>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName ="Add new invalid provider")]
        [Trait("Categoria","Fornecedor - Serviço")]
        public async Task Add_AddNewProviderInvalid_ShouldNotAddAndNotifyValidations()
        {
            //Arrange
            var provider = _providerTestFixture.GenerateInvalidProvider();
            var mocker = new AutoMocker();
            var serviceProvider = mocker.CreateInstance<ProviderService>();

            //Act
            await serviceProvider.AddAsyncUow(provider);

            //Assert
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.Add(It.IsAny<Provider>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);   
        }

        [Fact(DisplayName ="Add new valid provider with invalid contact")]
        [Trait("Categoria", "Fornecedor - Serviço")]
        public async Task Add_AddNewProviderWithInvalidContact_ShouldNotAddAndNotfyValidation()
        {
            //Arrange
            var provider = _providerTestFixture.GenerateProvider(1, true, false, false).FirstOrDefault();
            var mocker = new AutoMocker();
            var serviceProvider = mocker.CreateInstance<ProviderService>();

            //Act
            await serviceProvider.AddAsyncUow(provider);

            //Assert
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.Add(It.IsAny<Provider>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Exactly(4));
        }

        [Fact(DisplayName ="Add new valid provider with invalid addresses")]
        [Trait("Categoria","Fornecedor - Serviço")]
        public async Task Add_AddNewValidProviderWithValidContactsAndInvalidAddresses_ShouldNotAddAndNotifyValidation()
        {
            //Arrange
            var provider = _providerTestFixture.GenerateProvider(1, true, true, true, false).FirstOrDefault();
            var mocker = new AutoMocker();
            var serviceProvider = mocker.CreateInstance<ProviderService>();

            //Act
            await serviceProvider.AddAsyncUow(provider);

            //Assert
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.Add(It.IsAny<Provider>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Exactly(2));
        }

        [Fact(DisplayName ="Add new valid provider with exception")]
        [Trait("Cateoria","Fornecedor - Serviço")]
        public async Task Add_AddNewValidProviderWithException_ShouldNotAddAndNotifyErrorMessage()
        {
            //Arrange
            var provider = _providerTestFixture.GenerateValidProvider();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProvider>()
                  .Setup(r => r.Add(It.IsAny<Provider>()))
                  .Throws(new Exception());
            var serviceProvider = mocker.CreateInstance<ProviderService>();

            //Act
            await serviceProvider.AddAsyncUow(provider);

            //Assert
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.Add(It.IsAny<Provider>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            Assert.Throws<Exception>(()=>serviceProvider.Add(provider));
        }
        #endregion

        #region Function Get

        [Fact(DisplayName = "Get all providers with success")]
        [Trait("Categoria", "Fornecedor - Serviço")]
        public async Task GetAll_GetAllProviders_ShouldReturnAllProviders()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProvider>()
                  .Setup(r => r.GetAllProvidersAsync(It.IsAny<int>(), It.IsAny<int>()))
                  .Returns(Task.FromResult((IEnumerable<Provider>)_providerTestFixture.GenerateProvider(10)));
            var serviceProvider = mocker.CreateInstance<ProviderService>();

            //Act
            var providers = await serviceProvider.GetAllAsync(1, 1);

            //Assert
            providers.Should().NotBeNullOrEmpty();
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.GetAllProvidersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            providers.Should().HaveCount(10);
        }

        [Fact(DisplayName ="Get all providers with exception")]
        [Trait("Categoria", "Fornecedor - Serviço")]
        public async Task GetAll_GetAllProvideresWithException_ShouldNotReturnAndNotifyError()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProvider>()
                  .Setup(r => r.GetAllProvidersAsync(It.IsAny<int>(), It.IsAny<int>()))
                  .Throws(new Exception());
            var serviceProviders = mocker.CreateInstance<ProviderService>();

            //Act
            var providers = await serviceProviders.GetAllAsync(1, 1);

            //Assert
            Assert.Null(providers);
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.GetAllProvidersAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mocker.GetMock<INotifier>().Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Get provider by id with success")]
        [Trait("Categoria", "Fornecedor - Serviço")]
        public async Task GetById_GetProviderById_ShouldReturnProvider()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProvider>()
                  .Setup(r => r.GetProviderByIdAsync(It.IsAny<Guid>()))
                  .Returns(Task.FromResult(_providerTestFixture.GenerateValidProvider()));
            var serviceProvider = mocker.CreateInstance<ProviderService>();

            //Act
            var provider = await serviceProvider.GetByIdAsync(Guid.NewGuid());

            //Assert
            provider.Should().NotBeNull();
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.GetProviderByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact(DisplayName = "Get provider by id with exception")]
        [Trait("Categoria", "Fornecedor - Serviço")]
        public async Task GetById_GetProviderByIdWithException_ShouldNotReturnAndNotifyError()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProvider>()
                  .Setup(r => r.GetProviderByIdAsync(It.IsAny<Guid>()))
                  .Throws(new Exception());
            var serviceProviders = mocker.CreateInstance<ProviderService>();

            //Act
            var provider = await serviceProviders.GetByIdAsync(Guid.NewGuid());

            //Assert
            Assert.Null(provider);
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.GetProviderByIdAsync(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<INotifier>().Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Get provider by name with success")]
        [Trait("Categoria", "Fornecedor - Serviço")]
        public async Task GetByName_GetProviderByName_ShouldReturnProviders()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProvider>()
                  .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                  .Returns(Task.FromResult((IEnumerable<Provider>)_providerTestFixture.GenerateProvider(10)));
            var serviceProvider = mocker.CreateInstance<ProviderService>();

            //Act
            var providers = await serviceProvider.GetByNameAsync("AAaaa");

            //Assert
            providers.Should().NotBeNullOrEmpty();
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.GetByNameAsync(It.IsAny<string>()), Times.Once);
            providers.Should().HaveCount(10);
        }

        [Fact(DisplayName = "Get provider by name with exception")]
        [Trait("Categoria", "Fornecedor - Serviço")]
        public async Task GetByName_GetProviderByNameWithException_ShouldNotReturnAndNotifyError()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProvider>()
                  .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                  .Throws(new Exception());
            var serviceProviders = mocker.CreateInstance<ProviderService>();

            //Act
            var providers = await serviceProviders.GetByNameAsync("AAAaaa");

            //Assert
            Assert.Null(providers);
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.GetByNameAsync(It.IsAny<string>()), Times.Once);
            mocker.GetMock<INotifier>().Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Get provider by name with invalid name")]
        [Trait("Categoria", "Fornecedor - Serviço")]
        public async Task GetByName_GetProviderByNameWithInvalidName_ShouldNotReturnAndNotifyError()
        {
            //Arrange
            var mocker = new AutoMocker();
            var serviceProviders = mocker.CreateInstance<ProviderService>();

            //Act
            var providers = await serviceProviders.GetByNameAsync("");

            //Assert
            Assert.Null(providers);
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.GetByNameAsync(It.IsAny<string>()), Times.Never);
            mocker.GetMock<INotifier>().Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
        }
        #endregion

        #region Function Update

        [Fact(DisplayName ="Update valid provider with success")]
        [Trait("Categoria","Fornecedor - Serviço")]
        public async Task Update_UpdateValidProvider_ShouldUpdateWithSuccess()
        {
            //Arrange
            var provider = _providerTestFixture.GenerateValidProvider();
            var mocker = new AutoMocker();
            var serviceProvider = mocker.CreateInstance<ProviderService>();

            //Act 
            await serviceProvider.UpdateAsyncUow(provider);

            //Assert
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.Update(It.IsAny<Provider>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName ="Update invalid provider")]
        [Trait("Categoria","Fornecedor - Serviço")]
        public async Task Update_UpdateInvalidProvider_ShouldNotUpdateAndNotifyValidations()
        {
            //Arrange
            var provider = _providerTestFixture.GenerateInvalidProvider();
            var mocker = new AutoMocker();
            var serviceProvider = mocker.CreateInstance<ProviderService>();

            //Act
            await serviceProvider.UpdateAsyncUow(provider);

            //Assert
            mocker.GetMock<IRepositoryProvider>().Verify(r=>r.Update(It.IsAny<Provider>()),Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(r=>r.CommitAsync(),Times.Never);
            mocker.GetMock<INotifier>().Verify(r=>r.Handle(It.IsAny<Notification>()),Times.Once);
        }

        [Fact(DisplayName ="Update valid provider with invalid address")]
        [Trait("Categoria", "Fornecedor - Serviço")]
        public async Task Update_UpdateValidProviderWithInvalidAddress_ShouldNotUpdateAndNotifyValidations()
        {
            //Arrange
            var provider = _providerTestFixture.GenerateValidProvider(true, false);
            var mocker = new AutoMocker();
            var serviceProvider = mocker.CreateInstance<ProviderService>();

            //Act
            await serviceProvider.UpdateAsyncUow(provider);

            //Assert
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.Update(It.IsAny<Provider>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(n=>n.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n=>n.Handle(It.IsAny<Notification>()), Times.Exactly(4));
        }

        [Fact(DisplayName ="Update valid provider with invalid contact")]
        [Trait("Categoria", "Fornecedor - Serviço")]
        public async Task Update_UpdateValidProviderWithInvalidContact_ShouldNotUpdateAndNotifyValidations()
        {
            //Arrange
            var provider = _providerTestFixture.GenerateValidProvider(true, true, true, false);
            var mocker = new AutoMocker();
            var serviceProvider = mocker.CreateInstance<ProviderService>();

            //Act
            await serviceProvider.UpdateAsyncUow(provider);

            //Assert
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.Update(It.IsAny<Provider>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(n => n.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Exactly(2));
        }
        #endregion

        #region Function Remove

        [Fact(DisplayName ="Remove provider with success")]
        [Trait("Categoria", "Fornecedor - Serviço")]
        public async Task Remove_RemoveProvider_ShouldRemoveWithSuccess()
        {
            //Arrange
            var provider = _providerTestFixture.GenerateValidProvider();
            var mocker = new AutoMocker();
            var serviceProvider = mocker.CreateInstance<ProviderService>();

            //Act
            await serviceProvider.RemoveAsyncUow(provider);

            //Assert
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.Update(It.IsAny<Provider>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Remove provider with exception")]
        [Trait("Categoria", "Fornecedor - Serviço")]
        public async Task Remove_RemoveProviderWithException_ShouldNotRemove()
        {
            //Arrange
            var provider = _providerTestFixture.GenerateValidProvider();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryProvider>()
                  .Setup(r => r.Update(It.IsAny<Provider>()))
                  .Throws(new Exception());
            var serviceProvider = mocker.CreateInstance<ProviderService>();

            //Act
            await serviceProvider.RemoveAsyncUow(provider);

            //Assert
            mocker.GetMock<IRepositoryProvider>().Verify(r => r.Update(It.IsAny<Provider>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            
        }

        #endregion
    }
}