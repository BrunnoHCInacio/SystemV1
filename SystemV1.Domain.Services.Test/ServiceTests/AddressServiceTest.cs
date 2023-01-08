using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
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
    [Collection(nameof(AddressCollection))]
    public class AddressServiceTest : ServiceTestBase
    {
        private readonly AddressTestFixture _addressTestFixture;

        public AddressServiceTest(AddressTestFixture addressTestFixture) : base(new AutoMocker())
        {
            _addressTestFixture = addressTestFixture;
        }

        #region Função Adicionar

        [Fact(DisplayName = "Add new address with success")]
        [Trait("UnitTests - Services", "Address")]
        public async Task Add_AddNewAddressValid_ShouldAddWithSuccess()
        {
            //Arrange
            var address = _addressTestFixture.GenerateValidAddress();
            SetSetupMock<Address, IValidationAddress>(address);
            var serviceAddress = _autoMocker.CreateInstance<ServiceAddress>();

            //Act
            await serviceAddress.AddAsyncUow(address);

            //Assert

            _autoMocker.GetMock<IRepositoryAddress>().Verify(n => n.Add(It.IsAny<Address>()), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(n => n.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add new address with fail")]
        [Trait("UnitTests - Services", "Address")]
        public async Task Add_AddNewAddressInvalid_ShouldNotAdd()
        {
            //Arrange
            var address = _addressTestFixture.GenerateInvalidAddress();
            SetSetupMock<Address, IValidationAddress>(address, GenerateMockErrors("Street", ValidationAddress.StreetRequired));
            var serviceAddress = _autoMocker.CreateInstance<ServiceAddress>();

            //Act
            await serviceAddress.AddAsyncUow(address);

            //Assert
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }

        #endregion Função Adicionar

        #region Função modificar

        [Fact(DisplayName = "update address with success")]
        [Trait("UnitTests - Services", "Address")]
        public async Task Update_UpdateValidAddress_ShouldUpdatedWithSuccess()
        {
            //Arrange
            var address = _addressTestFixture.GenerateValidAddress();
            SetSetupMock<Address, IValidationAddress>(address);
            var serviceAddress = _autoMocker.CreateInstance<ServiceAddress>();

            //Act
            await serviceAddress.UpdateAsyncUow(address);

            //Assert
            _autoMocker.GetMock<IRepositoryAddress>().Verify(n => n.Update(It.IsAny<Address>()), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(n => n.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update invalid address")]
        [Trait("UnitTests - Services", "Address")]
        public async Task Update_UpdateInvalidAddress_ShouldNotUpdate()
        {
            //Arrange
            var address = _addressTestFixture.GenerateInvalidAddress();
            SetSetupMock<Address, IValidationAddress>(address, GenerateMockErrors("Street", ValidationAddress.StreetRequired));
            var serviceAddress = _autoMocker.CreateInstance<ServiceAddress>();

            //Act
            await serviceAddress.UpdateAsyncUow(address);

            //Assert
            _autoMocker.GetMock<IRepositoryAddress>().Verify(n => n.Update(It.IsAny<Address>()), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(n => n.CommitAsync(), Times.Never);
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
        }

        #endregion Função modificar

        #region Função Obter

        [Fact(DisplayName = "Get all addresses")]
        [Trait("UnitTests - Services", "Address")]
        public async Task GetAll_GetAllAddresses_ShouldReturnWithSuccess()
        {
            //Arrange
            var page = 1;
            var pageSize = 10;
            _autoMocker.GetMock<IRepositoryAddress>()
                  .Setup(r => r.SearchAsync(null, page, pageSize))
                  .Returns(Task.FromResult(_addressTestFixture.GenerateAddress(10)));
            var serviceAddress = _autoMocker.CreateInstance<ServiceAddress>();

            //Act
            var addresses = await serviceAddress.SearchAsync(null, page, pageSize);

            //Assert
            _autoMocker.GetMock<IRepositoryAddress>().Verify(n => n.SearchAsync(null, page, pageSize), Times.Once);
            addresses.Should().NotBeEmpty();
        }

        [Fact(DisplayName = "Get address by id with success")]
        [Trait("UnitTests - Services", "Address")]
        public async Task GetById_GetAddressById_ShouldReturnAddressWIthSuccess()
        {
            //Arrange
            var addressId = Guid.NewGuid();
            var serviceAddress = _autoMocker.CreateInstance<ServiceAddress>();
            _autoMocker.GetMock<IRepositoryAddress>()
                  .Setup(r => r.GetEntityAsync(a => a.Id == addressId, null))
                  .Returns(Task.FromResult(_addressTestFixture.GenerateValidAddress()));

            //Act
            var address = await serviceAddress.GetEntityAsync(a => a.Id == addressId, null);

            //Assert
            _autoMocker.GetMock<IRepositoryAddress>().Verify(n => n.GetEntityAsync(a => a.Id == addressId, null), Times.Once);
            address.Should().NotBeNull();
        }

        #endregion Função Obter

        #region Função Remover

        //Remove with valid address
        [Fact(DisplayName = "Remove address with success")]
        [Trait("UnitTests - Services", "Address")]
        public async Task Remove_RemoveAddressValid_ShouldUpdateWithSuccess()
        {
            //Arrange
            var address = _addressTestFixture.GenerateValidAddress();
            SetSetupMock<Address, IValidationAddress>(address);
            var serviceAddress = _autoMocker.CreateInstance<ServiceAddress>();

            //Act
            await serviceAddress.RemoveAsyncUow(address);

            //Assert
            _autoMocker.GetMock<IRepositoryAddress>().Verify(n => n.Remove(It.IsAny<Address>()), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(n => n.CommitAsync(), Times.Once);
        }

        #endregion Função Remover
    }
}