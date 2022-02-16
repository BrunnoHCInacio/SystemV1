using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections;
using System.Collections.Generic;
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
    [Collection(nameof(AddressCollection))]
    public class AddressServiceTest
    {
        private readonly AddressTestFixture _addressTestFixture;

        public AddressServiceTest(AddressTestFixture addressTestFixture)
        {
            _addressTestFixture = addressTestFixture;
        }

        #region Função Adicionar
        [Fact(DisplayName = "Add new address with success")]
        [Trait("Categoria", "Endereço - Serviço")]
        public async Task Add_AddNewAddressValid_ShouldAddWithSuccess()
        {
            //Arrange
            var address = _addressTestFixture.GenerateValidAddress();
            var mocker = new AutoMocker();
            var serviceAddress = mocker.CreateInstance<ServiceAddress>();
            mocker.GetMock<IUnitOfWork>().Setup(u => u.CommitAsync()).Returns(Task.FromResult(true));
            //Act
            await serviceAddress.AddAsyncUow(address);

            //Assert
            Assert.True(address.ValidateAddress().IsValid);

            mocker.GetMock<IRepositoryAddress>().Verify(n => n.Add(It.IsAny<Address>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(n => n.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add new address with fail")]
        [Trait("Categoria", "Endereço - Serviço")]
        public async Task Add_AddNewAddressInvalid_ShouldNotAdd()
        {
            //Arrange
            var address = _addressTestFixture.GenerateInvalidAddress();
            var mocker = new AutoMocker();
            var serviceAddress = mocker.CreateInstance<ServiceAddress>();

            //Act
            await serviceAddress.AddAsyncUow(address);

            //Assert
            Assert.False(address.ValidateAddress().IsValid);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }


        [Fact(DisplayName = "Add new address with exception")]
        [Trait("Categoria", "Endereço - Serviço")]
        public async Task Add_AddNewAddressValidWithException_ShouldNotAdd()
        {
            //Arrange
            var address = _addressTestFixture.GenerateValidAddress();
            var mocker = new AutoMocker();
            var serviceAddress = mocker.CreateInstance<ServiceAddress>();
            mocker.GetMock<IRepositoryAddress>()
                  .Setup(u => u.Add(It.IsAny<Address>()))
                  .Throws(new Exception());

            //Act
            await serviceAddress.AddAsyncUow(address);

            //Assert
            mocker.GetMock<IRepositoryAddress>().Verify(n => n.Add(It.IsAny<Address>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(n => n.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);

        }
        #endregion

        #region Função modificar

        [Fact(DisplayName = "update address with success")]
        [Trait("Categoria", "Endereço - Serviço")]
        public async Task Update_UpdateValidAddress_ShouldUpdatedWithSuccess()
        {
            //Arrange
            var address = _addressTestFixture.GenerateValidAddress();
            var mocker = new AutoMocker();
            mocker.GetMock<IUnitOfWork>().Setup(u => u.CommitAsync()).Returns(Task.FromResult(true));
            var serviceAddress = mocker.CreateInstance<ServiceAddress>();

            //Act
            await serviceAddress.UpdateAsyncUow(address);

            //Assert
            mocker.GetMock<IRepositoryAddress>().Verify(n => n.Update(It.IsAny<Address>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(n => n.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update invalid address" )]
        [Trait("Categoria", "Endereço - Serviço")]
        public async Task Update_UpdateInvalidAddress_ShouldNotUpdate()
        {
            //Arrange
            var address = _addressTestFixture.GenerateInvalidAddress();
            var mocker = new AutoMocker();
            var serviceAddress = mocker.CreateInstance<ServiceAddress>();
            
            //Act
            await serviceAddress.UpdateAsyncUow(address);

            //Assert
            mocker.GetMock<IRepositoryAddress>().Verify(n => n.Update(It.IsAny<Address>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(n => n.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Exactly(2));
        }

        [Fact(DisplayName = "Update address with exception")]
        [Trait("Categoria", "Endereço - Serviço")]
        public async Task Add_AddNewAddressWithException_ShouldNotUpdate()
        {
            //Arrange
            var address = _addressTestFixture.GenerateValidAddress();
            var mocker = new AutoMocker();
            var serviceAddress = mocker.CreateInstance<ServiceAddress>();
            mocker.GetMock<IRepositoryAddress>()
                  .Setup(r => r.Update(It.IsAny<Address>()))
                  .Throws(new Exception());
            //Act
            await serviceAddress.UpdateAsyncUow(address);

            //Assert
            mocker.GetMock<IRepositoryAddress>().Verify(n => n.Update(It.IsAny<Address>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(n => n.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }
        #endregion

        #region Função Obter
        [Fact(DisplayName = "Get all addresses")]
        [Trait("Categoria", "Endereço - Serviço")]
        public async Task GetAll_GetAllAddresses_ShouldReturnWithSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryAddress>()
                  .Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                  .Returns(Task.FromResult((IEnumerable<Address>)_addressTestFixture.GenerateAddress(10)));
            var serviceAddress = mocker.CreateInstance<ServiceAddress>();
            
            //Act
            var addresses = await serviceAddress.GetAllAsync(1,1);

            //Assert
            mocker.GetMock<IRepositoryAddress>().Verify(n => n.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            addresses.Should().NotBeEmpty();

        }

        [Fact(DisplayName = "Get all addresses with exception")]
        [Trait("Categoria", "Endereço - Serviço")]
        public async Task GetAll_GetAllAddressesWithException_ShouldNotReturnAddress()
        {
            //Arrange
            var address = _addressTestFixture.GenerateValidAddress();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryAddress>()
                  .Setup(u => u.GetAllAsync(It.IsAny<int>(),It.IsAny<int>()))
                  .Throws(new Exception());

            var serviceAddress = mocker.CreateInstance<ServiceAddress>();
            
            //Act
            var addresses = await serviceAddress.GetAllAsync(1,1);

            //Assert
            mocker.GetMock<IRepositoryAddress>().Verify(n => n.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            Assert.Null(addresses);
        }

        [Fact(DisplayName = "Get address by id with success")]
        [Trait("Categoria", "Endereço - Serviço")]
        public async Task GetById_GetAddressById_ShouldReturnAddressWIthSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            var serviceAddress = mocker.CreateInstance<ServiceAddress>();
            mocker.GetMock<IRepositoryAddress>()
                  .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .Returns(Task.FromResult(_addressTestFixture.GenerateValidAddress()));

            //Act
            var address = await serviceAddress.GetByIdAsync(Guid.NewGuid());

            //Assert
            mocker.GetMock<IRepositoryAddress>().Verify(n => n.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            address.Should().NotBeNull();
        }

        [Fact(DisplayName = "Get address by id with exception")]
        [Trait("Categoria", "Endereço - Serviço")]
        public async Task GetById_GetAddressByIdWithException_ShouldnotReturnAddress()
        {
            //Arrange
            var mocker = new AutoMocker();
            var serviceAddress = mocker.CreateInstance<ServiceAddress>();
            mocker.GetMock<IRepositoryAddress>()
                  .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .Throws(new Exception());

            //Act
            var address = await serviceAddress.GetByIdAsync(Guid.NewGuid());

            //Assert
            mocker.GetMock<IRepositoryAddress>().Verify(n => n.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            
            address.Should().BeNull();
        }

        #endregion

        #region Função Remover

        //Remove with valid address
        [Fact(DisplayName = "Remove address with success")]
        [Trait("Categoria", "Endereço - Serviço")]
        public async Task Remove_RemoveAddressValid_ShouldUpdateWithSuccess()
        {
            //Arrange
            var address = _addressTestFixture.GenerateValidAddress();
            var mocker = new AutoMocker();
            var serviceAddress = mocker.CreateInstance<ServiceAddress>();
            
            //Act
            await serviceAddress.RemoveAsyncUow(address);

            //Assert
            mocker.GetMock<IRepositoryAddress>().Verify(n => n.Update(It.IsAny<Address>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(n => n.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Remove address with exception")]
        [Trait("Categoria", "Endereço - Serviço")]
        public async Task Remove_RemoveAddressWithException_ShouldNotRemove()
        {
            //Arrange
            var address = _addressTestFixture.GenerateValidAddress();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryAddress>()
                  .Setup(u => u.Update(It.IsAny<Address>()))
                  .Throws(new Exception());
            var serviceAddress = mocker.CreateInstance<ServiceAddress>();
            
            //Act
            await serviceAddress.RemoveAsyncUow(address);

            //Assert
            mocker.GetMock<IRepositoryAddress>().Verify(n => n.Update(It.IsAny<Address>()), Times.Once);
            Assert.Throws<Exception>(() => serviceAddress.Remove(address));
            mocker.GetMock<IUnitOfWork>().Verify(n => n.CommitAsync(), Times.Never);
        }

        #endregion
    }
}