using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
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
    [Collection(nameof(ProviderCollection))]
    public class ProviderServiceTest : ServiceTestBase
    {
        private readonly ProviderTestFixture _providerTestFixture;

        public ProviderServiceTest(ProviderTestFixture providerTestFixture) : base(new AutoMocker())
        {
            _providerTestFixture = providerTestFixture;
        }

        #region Function Add

        [Fact(DisplayName = "Add new provider with success")]
        [Trait("UnitTests - Services", "Provider")]
        public async Task Add_AddNewProviderValid_MustAddWithSuccess()
        {
            //Arrange
            var provider = _providerTestFixture.GenerateValidProvider(Guid.NewGuid());
            SetSetupMock<Provider, IValidationProvider>(provider);
            var serviceProvider = _autoMocker.CreateInstance<ProviderService>();

            //Act
            await serviceProvider.AddAsyncUow(provider);

            //Assert
            _autoMocker.GetMock<IRepositoryProvider>().Verify(r => r.Add(It.IsAny<Provider>()), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add new invalid provider")]
        [Trait("UnitTests - Services", "Provider")]
        public async Task Add_AddNewProviderInvalid_MustNotAddAndNotifyValidations()
        {
            //Arrange
            var provider = _providerTestFixture.GenerateInvalidProvider();
            var errors = GenerateMockErrors("PeopleId", ValidationProvider.PeopleNotExist);
            SetSetupMock<Provider, IValidationProvider>(provider, errors);
            var serviceProvider = _autoMocker.CreateInstance<ProviderService>();

            //Act
            await serviceProvider.AddAsyncUow(provider);

            //Assert
            _autoMocker.GetMock<IRepositoryProvider>().Verify(r => r.Add(It.IsAny<Provider>()), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Exactly(1));
        }

        #endregion Function Add

        #region Function Get

        [Fact(DisplayName = "Get all providers with success")]
        [Trait("UnitTests - Services", "Provider")]
        public async Task GetAll_GetAllProviders_MustReturnAllProviders()
        {
            //Arrange
            var page = 1;
            var pageSize = 10;
            _autoMocker.GetMock<IRepositoryProvider>()
                  .Setup(r => r.SearchAsync(null, page, pageSize))
                  .Returns(Task.FromResult(_providerTestFixture.GenerateProvider(10, Guid.NewGuid())));
            var serviceProvider = _autoMocker.CreateInstance<ProviderService>();

            //Act
            var providers = await serviceProvider.SearchAsync(null, page, pageSize);

            //Assert
            providers.Should().NotBeNullOrEmpty();
            _autoMocker.GetMock<IRepositoryProvider>().Verify(r => r.SearchAsync(null, page, pageSize), Times.Once);
            providers.Should().HaveCount(10);
        }

        [Fact(DisplayName = "Get provider by id with success")]
        [Trait("UnitTests - Services", "Provider")]
        public async Task GetById_GetProviderById_MustReturnProvider()
        {
            //Arrange
            var providerId = Guid.NewGuid();
            _autoMocker.GetMock<IRepositoryProvider>()
                  .Setup(r => r.GetEntityAsync(p => p.Id == providerId, null))
                  .Returns(Task.FromResult(_providerTestFixture.GenerateValidProvider(Guid.NewGuid())));
            var serviceProvider = _autoMocker.CreateInstance<ProviderService>();

            //Act
            var provider = await serviceProvider.GetEntityAsync(p => p.Id == providerId, null);

            //Assert
            provider.Should().NotBeNull();
            _autoMocker.GetMock<IRepositoryProvider>().Verify(r => r.GetEntityAsync(p => p.Id == providerId, null), Times.Once);
        }

        #endregion Function Get

        #region Function Update

        [Fact(DisplayName = "Update valid provider with success")]
        [Trait("UnitTests - Services", "Provider")]
        public async Task Update_UpdateValidProvider_MustUpdateWithSuccess()
        {
            //Arrange
            var provider = _providerTestFixture.GenerateValidProvider(Guid.NewGuid());
            SetSetupMock<Provider, IValidationProvider>(provider);
            var serviceProvider = _autoMocker.CreateInstance<ProviderService>();

            //Act
            await serviceProvider.UpdateAsyncUow(provider);

            //Assert
            _autoMocker.GetMock<IRepositoryProvider>().Verify(r => r.Update(It.IsAny<Provider>()), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update invalid provider")]
        [Trait("UnitTests - Services", "Provider")]
        public async Task Update_UpdateInvalidProvider_MustNotUpdateAndNotifyValidations()
        {
            //Arrange
            var provider = _providerTestFixture.GenerateInvalidProvider();
            var errors = GenerateMockErrors("PeopleId", ValidationProvider.PeopleNotExist);
            SetSetupMock<Provider, IValidationProvider>(provider, errors);
            var serviceProvider = _autoMocker.CreateInstance<ProviderService>();

            //Act
            await serviceProvider.UpdateAsyncUow(provider);

            //Assert
            _autoMocker.GetMock<IRepositoryProvider>().Verify(r => r.Update(It.IsAny<Provider>()), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
            _autoMocker.GetMock<INotifier>().Verify(r => r.Handle(It.IsAny<Notification>()), Times.Exactly(1));
        }

        #endregion Function Update

        #region Function Remove

        [Fact(DisplayName = "Remove provider with success")]
        [Trait("UnitTests - Services", "Provider")]
        public async Task Remove_RemoveProvider_MustRemoveWithSuccess()
        {
            //Arrange
            var provider = _providerTestFixture.GenerateValidProvider(Guid.NewGuid());
            SetSetupMock<Provider, IValidationProvider>(provider);
            var serviceProvider = _autoMocker.CreateInstance<ProviderService>();

            //Act
            await serviceProvider.RemoveAsyncUow(provider);

            //Assert
            _autoMocker.GetMock<IRepositoryProvider>().Verify(r => r.Remove(It.IsAny<Provider>()), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        #endregion Function Remove
    }
}