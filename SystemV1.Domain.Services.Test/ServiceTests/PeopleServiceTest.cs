using System;
using System.Collections.Generic;
using System.Text;

namespace SystemV1.Domain.Test.ServiceTests
{
    internal class PeopleServiceTest
    {
        //[Fact(DisplayName = "Add new valid provider with invalid contact")]
        //[Trait("UnitTests - Services", "Provider")]
        //public async Task Add_AddNewProviderWithInvalidContact_MustNotAddAndNotfyValidation()
        //{
        //    //Arrange
        //    var provider = _providerTestFixture.GenerateProvider(1, Guid.NewGuid()).FirstOrDefault();

        //    var serviceProvider = _autoMocker.CreateInstance<ProviderService>();

        //    //Act
        //    await serviceProvider.AddAsyncUow(provider);

        //    //Assert
        //    _autoMocker.GetMock<IRepositoryProvider>().Verify(r => r.Add(It.IsAny<Provider>()), Times.Never);
        //    _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        //    _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Exactly(4));
        //}

        //[Fact(DisplayName = "Add new valid provider with invalid addresses")]
        //[Trait("UnitTests - Services", "Provider")]
        //public async Task Add_AddNewValidProviderWithValidContactsAndInvalidAddresses_MustNotAddAndNotifyValidation()
        //{
        //    //Arrange
        //    var provider = _providerTestFixture.GenerateProvider(1, Guid.NewGuid()).FirstOrDefault();

        //    var serviceProvider = _autoMocker.CreateInstance<ProviderService>();

        //    //Act
        //    await serviceProvider.AddAsyncUow(provider);

        //    //Assert
        //    _autoMocker.GetMock<IRepositoryProvider>().Verify(r => r.Add(It.IsAny<Provider>()), Times.Never);
        //    _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        //    _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Exactly(2));
        //}

        //[Fact(DisplayName = "Update valid provider with invalid address")]
        //[Trait("UnitTests - Services", "Provider")]
        //public async Task Update_UpdateValidProviderWithInvalidAddress_MustNotUpdateAndNotifyValidations()
        //{
        //    //Arrange
        //    var provider = _providerTestFixture.GenerateValidProvider(Guid.NewGuid());

        //    var serviceProvider = _autoMocker.CreateInstance<ProviderService>();

        //    //Act
        //    await serviceProvider.UpdateAsyncUow(provider);

        //    //Assert
        //    _autoMocker.GetMock<IRepositoryProvider>().Verify(r => r.Update(It.IsAny<Provider>()), Times.Never);
        //    _autoMocker.GetMock<IUnitOfWork>().Verify(n => n.CommitAsync(), Times.Never);
        //    _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Exactly(4));
        //}

        //[Fact(DisplayName = "Update valid provider with invalid contact")]
        //[Trait("UnitTests - Services", "Provider")]
        //public async Task Update_UpdateValidProviderWithInvalidContact_MustNotUpdateAndNotifyValidations()
        //{
        //    //Arrange
        //    var provider = _providerTestFixture.GenerateValidProvider(Guid.NewGuid());

        //    var serviceProvider = _autoMocker.CreateInstance<ProviderService>();

        //    //Act
        //    await serviceProvider.UpdateAsyncUow(provider);

        //    //Assert
        //    _autoMocker.GetMock<IRepositoryProvider>().Verify(r => r.Update(It.IsAny<Provider>()), Times.Never);
        //    _autoMocker.GetMock<IUnitOfWork>().Verify(n => n.CommitAsync(), Times.Never);
        //    _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Exactly(2));
        //}

        //[Fact(DisplayName = "Update provider without addresses and contacts with success")]
        //[Trait("UnitTests - Services", "Provider")]
        //public async Task Update_UpdateClientWithoutAddressesAndContacts_ShouldUpdateWithSuccess()
        //{
        //Arrange
        //var provider = _providerTestFixture.GenerateValidProvider();
        //provider.Addresses.Clear();
        //provider.Contacts.Clear();
        //
        //var serviceProvider = _autoMocker.CreateInstance<ProviderService>();

        ////Act
        //await serviceProvider.UpdateAsyncUow(provider);

        ////Assert
        //_autoMocker.GetMock<IRepositoryProvider>().Verify(r => r.Update(It.IsAny<Provider>()), Times.Once);

        //_autoMocker.GetMock<IServiceAddress>().Verify(r => r.RemoveAllByProviderId(It.IsAny<Guid>()), Times.Once);
        //_autoMocker.GetMock<IServiceContact>().Verify(r => r.RemoveAllByProviderId(It.IsAny<Guid>()), Times.Once);

        //_autoMocker.GetMock<IServiceAddress>().Verify(r => r.AddAsync(It.IsAny<Address>()), Times.Never);
        //_autoMocker.GetMock<IServiceContact>().Verify(r => r.AddAsync(It.IsAny<Contact>()), Times.Never);

        //_autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        //}
    }
}