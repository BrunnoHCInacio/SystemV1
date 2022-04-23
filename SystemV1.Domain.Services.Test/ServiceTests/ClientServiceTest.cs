using FluentAssertions;
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
    [Collection(nameof(ClientCollection))]
    public class ClientServiceTest
    {
        private readonly ClientTestFixture _clientTestFixture;

        public ClientServiceTest(ClientTestFixture clientTestFixture)
        {
            _clientTestFixture = clientTestFixture;
        }

        #region Função Adicionar

        [Fact(DisplayName = "Add new client with valid contacts and valid addresses")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Add_AddNewClient_ShouldAddWithSuccess()
        {
            //Arrange
            var client = _clientTestFixture.GenerateValidClient();
            var mocker = new AutoMocker();
            var serviceclient = mocker.CreateInstance<ServiceClient>();
            mocker.GetMock<IUnitOfWork>().Setup(u => u.CommitAsync()).Returns(Task.FromResult(true));

            //Act
            await serviceclient.AddAsyncUow(client);

            //Assert
            Assert.True(client.ValidateClient().IsValid);
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Add(It.IsAny<Client>()), Times.Once);
            mocker.GetMock<IServiceAddress>().Verify(r => r.Add(It.IsAny<Address>()), Times.Exactly(client.Addresses.Count));
            mocker.GetMock<IServiceContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Exactly(client.Contacts.Count));
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add new valid client with valid addresses and invalid contacts")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Add_AddNewClientWithInvalidContacts_ShouldNotAddClientNotAddContactsAndNotifyMessage()
        {
            //Arrange
            var client = _clientTestFixture.GenerateClient(1, true, true, true, false).FirstOrDefault();
            var mocker = new AutoMocker();
            var serviceclient = mocker.CreateInstance<ServiceClient>();

            //Act
            await serviceclient.AddAsyncUow(client);

            //Assert
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Add(It.IsAny<Client>()), Times.Never);
            mocker.GetMock<IServiceAddress>().Verify(r => r.Add(It.IsAny<Address>()), Times.Never);
            mocker.GetMock<IServiceContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Add new valid client with valid contacts and invalid addresses")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Add_AddNewClientWithInvalidAddress_ShouldNotAddClientNotAddAddressAndNotifyMessage()
        {
            //Arrange
            var client = _clientTestFixture.GenerateClient(1, true, false, true, true).FirstOrDefault();
            var mocker = new AutoMocker();
            var serviceclient = mocker.CreateInstance<ServiceClient>();

            //Act
            await serviceclient.AddAsyncUow(client);

            //Assert
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Add(It.IsAny<Client>()), Times.Never);
            mocker.GetMock<IServiceAddress>().Verify(r => r.Add(It.IsAny<Address>()), Times.Never);
            mocker.GetMock<IServiceContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Add new only client with success")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Add_AddNewClient_ShouldAddOnlyClient()
        {
            //Arrange
            var client = _clientTestFixture.GenerateClient(1, false, false, false, false).FirstOrDefault();
            var mocker = new AutoMocker();
            var serviceclient = mocker.CreateInstance<ServiceClient>();
            mocker.GetMock<IUnitOfWork>().Setup(u => u.CommitAsync()).Returns(Task.FromResult(true));

            //Act
            await serviceclient.AddAsyncUow(client);

            //Assert
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Add(It.IsAny<Client>()), Times.Once);
            mocker.GetMock<IServiceAddress>().Verify(r => r.Add(It.IsAny<Address>()), Times.Never);
            mocker.GetMock<IServiceContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add new client with exception on client repository ")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Add_AddNewClientWithExceptionOnClientRepository_ShouldNotAddClientContactAddressAndNotifyMessage()
        {
            //Arrange
            var client = _clientTestFixture.GenerateValidClient();
            var mocker = new AutoMocker();
            var serviceclient = mocker.CreateInstance<ServiceClient>();
            mocker.GetMock<IRepositoryClient>()
                  .Setup(r => r.Add(It.IsAny<Client>()))
                  .Throws(new Exception());

            //Act
            await serviceclient.AddAsyncUow(client);

            //Assert
            mocker.GetMock<IServiceAddress>().Verify(r => r.Add(It.IsAny<Address>()), Times.Never);
            mocker.GetMock<IServiceContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            Assert.Throws<Exception>(() => serviceclient.Add(client));
        }

        [Fact(DisplayName = "Add new client with exception on address service ")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Add_AddNewClientWithExceptionOnAddressRepository_ShouldNotAddClientContactAddressAndNotifyMessage()
        {
            //Arrange
            var client = _clientTestFixture.GenerateValidClient();
            var mocker = new AutoMocker();
            var serviceclient = mocker.CreateInstance<ServiceClient>();
            mocker.GetMock<IServiceAddress>()
                  .Setup(r => r.Add(It.IsAny<Address>()))
                  .Throws(new Exception());

            //Act
            await serviceclient.AddAsyncUow(client);

            //Assert
           
            mocker.GetMock<IServiceContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            Assert.Throws<Exception>(() => serviceclient.Add(client));
        }

        [Fact(DisplayName = "Add new client with exception on contact service ")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Add_AddNewClientWithExceptionOnContactService_ShouldNotAddClientContactAddressAndNotifyMessage()
        {
            //Arrange
            var client = _clientTestFixture.GenerateValidClient();
            var mocker = new AutoMocker();
            var serviceclient = mocker.CreateInstance<ServiceClient>();
            mocker.GetMock<IServiceContact>()
                  .Setup(r => r.Add(It.IsAny<Contact>()))
                  .Throws(new Exception());

            //Act
            await serviceclient.AddAsyncUow(client);

            //Assert
            mocker.GetMock<IServiceAddress>().Verify(r => r.Add(It.IsAny<Address>()), Times.Exactly(client.Addresses.Count));
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            Assert.Throws<Exception>(() => serviceclient.Add(client));
        }
        #endregion

        #region Function Update

        [Fact(DisplayName = "Update client with addresses and contacts with success")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Update_UpdateClientWithAddressesAndContacts_ShouldUpdateWithSuccess()
        {
            //Arrange
            var client = _clientTestFixture.GenerateValidClient();
            var mocker = new AutoMocker();
            var serviceclient = mocker.CreateInstance<ServiceClient>();
           
            //Act
            await serviceclient.UpdateAsyncUow(client);

            //Assert
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Update(It.IsAny<Client>()), Times.Once);

            mocker.GetMock<IServiceAddress>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<IServiceAddress>().Verify(r => r.Add(It.IsAny<Address>()), Times.Exactly(client.Addresses.Count));

            mocker.GetMock<IServiceContact>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<IServiceContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Exactly(client.Contacts.Count));
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update client without addresses and contacts with success")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Update_UpdateClientWithoutAddressesAndContacts_ShouldUpdateWithSuccess()
        {
            //Arrange
            var client = _clientTestFixture.GenerateValidClient();
            client.Addresses.Clear();
            client.Contacts.Clear();
            var mocker = new AutoMocker();
            var serviceclient = mocker.CreateInstance<ServiceClient>();

            //Act
            await serviceclient.UpdateAsyncUow(client);

            //Assert
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Update(It.IsAny<Client>()), Times.Once);

            mocker.GetMock<IServiceAddress>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<IServiceAddress>().Verify(r => r.Add(It.IsAny<Address>()), Times.Never);

            mocker.GetMock<IServiceContact>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<IServiceContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update invalid client")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Update_UpdateInvalidClient_ShouldNotUpdateClient()
        {
            //Arrange
            var client = _clientTestFixture.GenerateInvalidClient();
            var mocker = new AutoMocker();
            var serviceclient = mocker.CreateInstance<ServiceClient>();

            //Act
            await serviceclient.UpdateAsyncUow(client);

            //Assert
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Update(It.IsAny<Client>()), Times.Never);

            mocker.GetMock<IServiceAddress>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Never);
            mocker.GetMock<IServiceAddress>().Verify(r => r.Add(It.IsAny<Address>()), Times.Never);

            mocker.GetMock<IServiceContact>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Never);
            mocker.GetMock<IServiceContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Exactly(2));
        }

        [Fact(DisplayName = "Update valid client with invalid address")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Update_UpdateValidClientWithInvalidAddress_ShouldNotUptadeClientAndAddress()
        {
            //Arrange
            var client = _clientTestFixture.GenerateClient(1,true, false, false,false).FirstOrDefault();
            var mocker = new AutoMocker();
            var serviceclient = mocker.CreateInstance<ServiceClient>();

            //Act
            await serviceclient.UpdateAsyncUow(client);

            //Assert
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Update(It.IsAny<Client>()), Times.Never);

            mocker.GetMock<IServiceAddress>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Never);
            mocker.GetMock<IServiceAddress>().Verify(r => r.Add(It.IsAny<Address>()), Times.Never);

            mocker.GetMock<IServiceContact>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Never);
            mocker.GetMock<IServiceContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Exactly(2));
        }

        [Fact(DisplayName = "Update valid client with invalid Contacts")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Update_UpdateValidClientWithInvalidContacts_ShouldNotUptadeClientAndContacts()
        {
            //Arrange
            var client = _clientTestFixture.GenerateClient(1, false, false, true, false).FirstOrDefault();
            var mocker = new AutoMocker();
            var serviceclient = mocker.CreateInstance<ServiceClient>();

            //Act
            await serviceclient.UpdateAsyncUow(client);

            //Assert
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Update(It.IsAny<Client>()), Times.Never);

            mocker.GetMock<IServiceAddress>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Never);
            mocker.GetMock<IServiceAddress>().Verify(r => r.Add(It.IsAny<Address>()), Times.Never);

            mocker.GetMock<IServiceContact>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Never);
            mocker.GetMock<IServiceContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Exactly(4));
        }

        [Fact(DisplayName = "Update valid client, contacts and addresses with exception on address service")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Update_UpdateValidClientWithExceptionOnServiceAddress_ShouldNotUptadeClientContactAndAddress()
        {
            //Arrange
            var client = _clientTestFixture.GenerateValidClient();
            var mocker = new AutoMocker();
            mocker.GetMock<IServiceAddress>()
                  .Setup(r => r.Add(It.IsAny<Address>()))
                  .Throws(new Exception());

            var serviceclient = mocker.CreateInstance<ServiceClient>();

            //Act
            await serviceclient.UpdateAsyncUow(client);

            //Assert
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Update(It.IsAny<Client>()), Times.Never);

            mocker.GetMock<IServiceAddress>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<IServiceContact>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);

            mocker.GetMock<IServiceAddress>().Verify(r => r.Add(It.IsAny<Address>()), Times.Once);
            mocker.GetMock<IServiceContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Update valid client, contacts and addresses with exception on service contact")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Update_UpdateValidClientWithExceptionOnServiceContact_ShouldNotUptadeClientContactAndAddress()
        {
            //Arrange
            var client = _clientTestFixture.GenerateValidClient();
            var mocker = new AutoMocker();
            mocker.GetMock<IServiceContact>()
                  .Setup(r => r.Add(It.IsAny<Contact>()))
                  .Throws(new Exception());

            var serviceclient = mocker.CreateInstance<ServiceClient>();

            //Act
            await serviceclient.UpdateAsyncUow(client);

            //Assert
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Update(It.IsAny<Client>()), Times.Never);

            mocker.GetMock<IServiceAddress>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<IServiceAddress>().Verify(r => r.Add(It.IsAny<Address>()), Times.Exactly(client.Addresses.Count()));

            mocker.GetMock<IServiceContact>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<IServiceContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Update valid client, contacts and addresses with exception on repository client")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Update_UpdateValidClientWithExceptionOnRepositoryClient_ShouldNotUptadeClientContactAndAddress()
        {
            //Arrange
            var client = _clientTestFixture.GenerateValidClient();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryClient>()
                  .Setup(r => r.Update(It.IsAny<Client>()))
                  .Throws(new Exception());

            var serviceclient = mocker.CreateInstance<ServiceClient>();

            //Act
            await serviceclient.UpdateAsyncUow(client);

            //Assert
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Update(It.IsAny<Client>()), Times.Once);

            mocker.GetMock<IServiceAddress>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<IServiceAddress>().Verify(r => r.Add(It.IsAny<Address>()), Times.Exactly(client.Addresses.Count()));

            mocker.GetMock<IServiceContact>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<IServiceContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Exactly(client.Contacts.Count()));
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }
        #endregion

        #region Função Obter

        [Fact(DisplayName = "Get all clients with success")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task GetAll_GetAllContacts_ShouldHasSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryClient>()
                  .Setup(r => r.GetAllClientsAsync(It.IsAny<int>(), It.IsAny<int>()))
                  .Returns(Task.FromResult((IEnumerable<Client>)_clientTestFixture.GenerateClient(5)));
            var serviceClient = mocker.CreateInstance<ServiceClient>();

            //Act
            var clients = await serviceClient.GetAllAsync(1, 1);

            //Assert
            clients.Should().NotBeEmpty();
            Assert.Equal(5, clients.Count());
            mocker.GetMock<IRepositoryClient>()
                  .Verify(r => r.GetAllClientsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Get all clients with exception")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task GetAll_GetAllContactsWithException_ShouldNotReturnListClients()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryClient>()
                  .Setup(r => r.GetAllClientsAsync(It.IsAny<int>(), It.IsAny<int>()))
                  .Throws(new Exception());
            var serviceClient = mocker.CreateInstance<ServiceClient>();

            //Act
            var clients = await serviceClient.GetAllAsync(1, 1);

            //Assert
            clients.Should().BeNull();
            mocker.GetMock<IRepositoryClient>()
                  .Verify(r => r.GetAllClientsAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mocker.GetMock<INotifier>()
                  .Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Get client by id with success")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task GetById_GetClientById_ShouldRetornClientWithSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryClient>()
                  .Setup(r => r.GetClientByIdAsync(It.IsAny<Guid>()))
                  .Returns(Task.FromResult(_clientTestFixture.GenerateValidClient()));
            var serviceClient = mocker.CreateInstance<ServiceClient>();

            //Act
            var client = await serviceClient.GetByIdAsync(Guid.NewGuid());

            //Assert
            client.Should().NotBeNull();
            mocker.GetMock<IRepositoryClient>()
                  .Verify(r => r.GetClientByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact(DisplayName = "Get client by id with exception")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task GetById_GetClientByIdWithException_ShouldNotReturnClient()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryClient>()
                  .Setup(r => r.GetClientByIdAsync(It.IsAny<Guid>()))
                  .Throws(new Exception());
            var serviceClient = mocker.CreateInstance<ServiceClient>();

            //Act
            var client = await serviceClient.GetByIdAsync(Guid.NewGuid());

            //Assert
            client.Should().BeNull();
            mocker.GetMock<IRepositoryClient>()
                  .Verify(r => r.GetClientByIdAsync(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<INotifier>()
                  .Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Get client by name with success")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task GetByName_GetClientByName_ShouldRetornClientWithSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryClient>()
                  .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                  .Returns(Task.FromResult((IEnumerable<Client>)_clientTestFixture.GenerateClient(5)));
            var serviceClient = mocker.CreateInstance<ServiceClient>();

            //Act
            var client = await serviceClient.GetByNameAsync("AAA");

            //Assert
            client.Should().NotBeEmpty();
            mocker.GetMock<IRepositoryClient>()
                  .Verify(r => r.GetByNameAsync(It.IsAny<String>()), Times.Once);
        }

        [Fact(DisplayName = "Get client by name with exception")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task GetByName_GetClientByNameWithException_ShouldNotReturnClient()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryClient>()
                  .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                  .Throws(new Exception());
            var serviceClient = mocker.CreateInstance<ServiceClient>();

            //Act
            var client = await serviceClient.GetByNameAsync("AAA");

            //Assert
            client.Should().BeNull();
            mocker.GetMock<IRepositoryClient>()
                  .Verify(r => r.GetByNameAsync(It.IsAny<String>()), Times.Once);
            mocker.GetMock<INotifier>()
                  .Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Get client by invalid name")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task GetByName_GetClientByInvalidName_ShouldNotReturnClient()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryClient>()
                  .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                  .Throws(new Exception());
            var serviceClient = mocker.CreateInstance<ServiceClient>();

            //Act
            var client = await serviceClient.GetByNameAsync("");

            //Assert
            client.Should().BeNull();
            mocker.GetMock<IRepositoryClient>()
                  .Verify(r => r.GetByNameAsync(It.IsAny<String>()), Times.Never);
            mocker.GetMock<INotifier>()
                  .Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
        }
        #endregion

        #region Função Remover
        [Fact(DisplayName = "Remove client without addresses and contacts with success")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Remove_RemoveClientWithOutAddressesAndContacts_ShouldRemoveWithSuccess()
        {
            //Arrange
            var client = _clientTestFixture.GenerateClient(1, false, false, false, false).FirstOrDefault();
            var mocker = new AutoMocker();
            var serviceclient = mocker.CreateInstance<ServiceClient>();

            //Act
            await serviceclient.RemoveAsyncUow(client);

            //Assert
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Update(It.IsAny<Client>()), Times.Once);

            mocker.GetMock<IServiceAddress>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Never);
            
            mocker.GetMock<IServiceContact>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Never);
            
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Remove client with addresses and with success")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Remove_RemoveClientWithAddresses_ShouldRemoveWithSuccess()
        {
            //Arrange
            var client = _clientTestFixture.GenerateClient(1, true, true, false, false)
                                           .FirstOrDefault();
            var mocker = new AutoMocker();
            var serviceclient = mocker.CreateInstance<ServiceClient>();

            //Act
            await serviceclient.RemoveAsyncUow(client);

            //Assert
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Update(It.IsAny<Client>()), Times.Once);

            mocker.GetMock<IServiceAddress>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);

            mocker.GetMock<IServiceContact>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Never);

            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Remove client with contacts and with success")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Remove_RemoveClientWithContacts_ShouldRemoveWithSuccess()
        {
            //Arrange
            var client = _clientTestFixture.GenerateClient(1, false, false, true, true)
                                           .FirstOrDefault();
            var mocker = new AutoMocker();
            var serviceclient = mocker.CreateInstance<ServiceClient>();

            //Act
            await serviceclient.RemoveAsyncUow(client);

            //Assert
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Update(It.IsAny<Client>()), Times.Once);

            mocker.GetMock<IServiceAddress>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Never);

            mocker.GetMock<IServiceContact>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);

            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Remove client with exception on client repository")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Remove_RemoveClientWithException_ShouldNotRemove()
        {
            //Arrange
            var client = _clientTestFixture.GenerateValidClient();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryClient>()
                  .Setup(r => r.Update(It.IsAny<Client>()))
                  .Throws(new Exception());

            var serviceclient = mocker.CreateInstance<ServiceClient>();

            //Act
            await serviceclient.RemoveAsyncUow(client);

            //Assert
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Update(It.IsAny<Client>()), Times.Once);
            mocker.GetMock<IServiceAddress>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<IServiceContact>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(u => u.Handle(It.IsAny<Notification>()), Times.Once);

            Assert.Throws<Exception>(()=>serviceclient.Remove(client));
        }

        [Fact(DisplayName = "Remove client with exception on address service")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Remove_RemoveClientWithExceptionOnAddressService_ShouldNotRemove()
        {
            //Arrange
            var client = _clientTestFixture.GenerateValidClient();
            var mocker = new AutoMocker();
            mocker.GetMock<IServiceAddress>()
                  .Setup(r => r.RemoveAllByClientId(It.IsAny<Guid>()))
                  .Throws(new Exception());

            var serviceclient = mocker.CreateInstance<ServiceClient>();

            //Act
            await serviceclient.RemoveAsyncUow(client);

            //Assert
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Update(It.IsAny<Client>()), Times.Never);
            mocker.GetMock<IServiceAddress>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<IServiceContact>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(u => u.Handle(It.IsAny<Notification>()), Times.Once);

            Assert.Throws<Exception>(() => serviceclient.Remove(client));
        }

        [Fact(DisplayName = "Remove client with exception on contact service")]
        [Trait("Categoria", "Cliente - Serviço")]
        public async Task Remove_RemoveClientWithExceptionOnContactService_ShouldNotRemove()
        {
            //Arrange
            var client = _clientTestFixture.GenerateValidClient();
            var mocker = new AutoMocker();
            mocker.GetMock<IServiceContact>()
                  .Setup(r => r.RemoveAllByClientId(It.IsAny<Guid>()))
                  .Throws(new Exception());

            var serviceclient = mocker.CreateInstance<ServiceClient>();

            //Act
            await serviceclient.RemoveAsyncUow(client);

            //Assert
            mocker.GetMock<IRepositoryClient>().Verify(r => r.Update(It.IsAny<Client>()), Times.Never);
            mocker.GetMock<IServiceAddress>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<IServiceContact>().Verify(r => r.RemoveAllByClientId(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(u => u.Handle(It.IsAny<Notification>()), Times.Once);

            Assert.Throws<Exception>(() => serviceclient.Remove(client));
        }

        #endregion

    }
}