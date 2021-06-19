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

        [Fact(DisplayName = "Add new client with success")]
        [Trait("Categoria", "Serviço - Cliente")]
        public async Task ClienteService_NewClient_ShouldBeSuccess()
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

        [Fact(DisplayName = "Add new valid client and contacts with fail")]
        [Trait("Categoria", "Serviço - Cliente")]
        public async Task ClienteService_NewClientAndInvalidContacts_ShouldBeFail()
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

        [Fact(DisplayName = "Add new valid client and invalid addresss with fail")]
        [Trait("Categoria", "Serviço - Cliente")]
        public async Task ClienteService_NewClientAnd_ShouldBeFail()
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

        [Fact(DisplayName = "Add new client with success")]
        [Trait("Categoria", "Serviço - Cliente")]
        public async Task ClienteService_NewClient_ShouldHasSuccess()
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
    }
}