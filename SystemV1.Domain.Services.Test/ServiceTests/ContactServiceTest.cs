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
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Enums;
using SystemV1.Domain.Services;
using SystemV1.Domain.Services.Notifications;
using SystemV1.Domain.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.ServiceTests
{
    [Collection(nameof(ContactCollection))]
    public class ContactServiceTest
    {
        private readonly ContactTestFixture _contactTestFixture;

        public ContactServiceTest(ContactTestFixture contactTestFixture)
        {
            _contactTestFixture = contactTestFixture;
        }

        #region Função Adicionar

        [Fact(DisplayName = "Add contact email with success")]
        [Trait("Categoria", "Serviço - Contato")]
        public async Task ContactService_NewContactTypeEmail_ShouldHaveSuccess()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeEmail();
            var mocker = new AutoMocker();
            var serviceContact = mocker.CreateInstance<ServiceContact>();
            mocker.GetMock<IUnitOfWork>().Setup(u => u.CommitAsync()).Returns(Task.FromResult(true));

            //Act
            await serviceContact.AddAsyncUow(contact);

            //Assert
            Assert.True(contact.ValidateContact().IsValid);
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add contact phone with success")]
        [Trait("Categoria", "Serviço - Contato")]
        public async Task ContactService_NewContactTypePhone_ShouldHaveSuccess()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypePhone();
            var mocker = new AutoMocker();
            var serviceContact = mocker.CreateInstance<ServiceContact>();
            mocker.GetMock<IUnitOfWork>().Setup(u => u.CommitAsync()).Returns(Task.FromResult(true));

            //Act
            await serviceContact.AddAsyncUow(contact);

            //Assert
            Assert.True(contact.ValidateContact().IsValid);
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add contact cell phone with success")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_NewContactTypeCellPhone_ShouldHaveSuccess()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeCellPhone();
            var mocker = new AutoMocker();
            var serviceContact = mocker.CreateInstance<ServiceContact>();
            mocker.GetMock<IUnitOfWork>().Setup(u => u.CommitAsync()).Returns(Task.FromResult(true));

            //Act
            await serviceContact.AddAsyncUow(contact);

            //Assert
            Assert.True(contact.ValidateContact().IsValid);
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add contact email with fail")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_NewContactTypeEmail_ShouldHaveFail()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateInvalidEmailContactWithInvalidProperies();
            var mocker = new AutoMocker();
            var serviceContact = mocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.AddAsyncUow(contact);

            //Assert
            Assert.False(contact.ValidateContact().IsValid);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Add contact phone with fail")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_NewContactTypePhone_ShouldHaveFail()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateInvalidPhoneContactWithInvalidProperties();
            var mocker = new AutoMocker();
            var serviceContact = mocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.AddAsyncUow(contact);

            //Assert
            Assert.False(contact.ValidateContact().IsValid);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Add contact cell phone with fail")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_NewContactTypeCellPhone_ShouldHaveFail()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateInvalidCellPhoneContactWithInvalidProperies();
            var mocker = new AutoMocker();
            var serviceContact = mocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.AddAsyncUow(contact);

            //Assert
            Assert.False(contact.ValidateContact().IsValid);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Add contact email with exception")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_NewContactTypeEmailWithException_ShouldFailed()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeEmail();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryContact>()
                  .Setup(r => r.Add(It.IsAny<Contact>()))
                  .Throws(new Exception());
            var serviceContact = mocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.AddAsyncUow(contact);

            //Assert
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()),Times.Once);
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
            Assert.Throws<Exception>(() => serviceContact.Add(contact));
        }

        [Fact(DisplayName = "Add contact phone with exception")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_NewContactTypePhoneWithException_ShouldFailed()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypePhone();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryContact>()
                  .Setup(r => r.Add(It.IsAny<Contact>()))
                  .Throws(new Exception());
            var serviceContact = mocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.AddAsyncUow(contact);

            //Assert
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
            Assert.Throws<Exception>(() => serviceContact.Add(contact));
        }

        [Fact(DisplayName = "Add contact cell phone with fail")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_NewContactTypeCellPhoneWithException_ShouldFailed()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeCellPhone();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryContact>()
                  .Setup(r => r.Add(It.IsAny<Contact>()))
                  .Throws(new Exception());
            var serviceContact = mocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.AddAsyncUow(contact);

            //Assert
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
            Assert.Throws<Exception>(() => serviceContact.Add(contact));
        }

        #endregion

        #region Função Modificar

        [Fact(DisplayName = "Update contact email with success")]
        [Trait("Categoria", "Serviço - Contato")]
        public async Task ContactService_UpdateContactTypeEmail_ShouldHaveSuccess()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeEmail();
            var mocker = new AutoMocker();
            var serviceContact = mocker.CreateInstance<ServiceContact>();
            mocker.GetMock<IUnitOfWork>().Setup(u => u.CommitAsync()).Returns(Task.FromResult(true));

            //Act
            await serviceContact.UpdateAsyncUow(contact);

            //Assert
            Assert.True(contact.ValidateContact().IsValid);
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Update(It.IsAny<Contact>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update contact phone with success")]
        [Trait("Categoria", "Serviço - Contato")]
        public async Task ContactService_UpdateContactTypePhone_ShouldHaveSuccess()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypePhone();
            var mocker = new AutoMocker();
            mocker.GetMock<IUnitOfWork>()
                  .Setup(u => u.CommitAsync())
                  .Returns(Task.FromResult(true));
            var serviceContact = mocker.CreateInstance<ServiceContact>();
            
            //Act
            await serviceContact.UpdateAsyncUow(contact);

            //Assert
            Assert.True(contact.ValidateContact().IsValid);
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Update(It.IsAny<Contact>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update contact cell phone with success")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_UpdateContactTypeCellPhone_ShouldHaveSuccess()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeCellPhone();
            var mocker = new AutoMocker();
            mocker.GetMock<IUnitOfWork>()
                  .Setup(u => u.CommitAsync())
                  .Returns(Task.FromResult(true));
            var serviceContact = mocker.CreateInstance<ServiceContact>();
            
            //Act
            await serviceContact.UpdateAsyncUow(contact);

            //Assert
            Assert.True(contact.ValidateContact().IsValid);
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Update(It.IsAny<Contact>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update contact email with fail")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_NewContactTypeEmail_ShouldFailed()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateInvalidEmailContactWithInvalidProperies();
            var mocker = new AutoMocker();
            var serviceContact = mocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.UpdateAsyncUow(contact);

            //Assert
            Assert.False(contact.ValidateContact().IsValid);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Update(It.IsAny<Contact>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Update contact phone with fail")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_UpdateContactTypePhone_ShouldFailed()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateInvalidPhoneContactWithInvalidProperties();
            var mocker = new AutoMocker();
            var serviceContact = mocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.UpdateAsyncUow(contact);

            //Assert
            Assert.False(contact.ValidateContact().IsValid);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Update(It.IsAny<Contact>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Update contact cell phone with fail")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_UpdateContactTypeCellPhone_ShouldFailed()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateInvalidCellPhoneContactWithInvalidProperies();
            var mocker = new AutoMocker();
            var serviceContact = mocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.UpdateAsyncUow(contact);

            //Assert
            Assert.False(contact.ValidateContact().IsValid);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Update(It.IsAny<Contact>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Update contact phone with exception")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_UpdateContactTypePhoneWithException_ShouldFailed()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypePhone();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryContact>()
                  .Setup(r => r.Update(It.IsAny<Contact>()))
                  .Throws(new Exception());
            var serviceContact = mocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.UpdateAsyncUow(contact);

            //Assert
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Update(It.IsAny<Contact>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
            Assert.Throws<Exception>(() => serviceContact.Update(contact));
        }
        #endregion

        #region Função Obter

        [Fact(DisplayName = "Get all contacts with success")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_GetAllContacts_ShouldHasSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryContact>()
                  .Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                  .Returns(Task.FromResult((IEnumerable<Contact>)_contactTestFixture.GenerateContact(EnumTypeContact.TypeContactPhone, 5)));
            var serviceContact = mocker.CreateInstance<ServiceContact>();

            //Act
            var contacts = await serviceContact.GetAllAsync(1,1);

            //Assert
            contacts.Should().NotBeEmpty();
            Assert.Equal(5, contacts.Count());
            mocker.GetMock<IRepositoryContact>().Verify(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact(DisplayName = "Get contact by id with success")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_GetContactById_ShouldHasSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryContact>()
                  .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .Returns(Task.FromResult(_contactTestFixture.GenerateValidContactTypeCellPhone()));
            var serviceContact = mocker.CreateInstance<ServiceContact>();

            //Act
            var contact = await serviceContact.GetByIdAsync(Guid.NewGuid());

            //Assert
            contact.Should().NotBeNull();
            mocker.GetMock<IRepositoryContact>().Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact(DisplayName = "Get all contacts with exception")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_GetAllContactsWithException_ShouldFailed()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryContact>()
                  .Setup(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()))
                  .Throws(new Exception());
            var serviceContact = mocker.CreateInstance<ServiceContact>();

            //Act
            var contact = await serviceContact.GetAllAsync(1, 1);

            //Assert
            contact.Should().BeNull();
            mocker.GetMock<IRepositoryContact>().Verify(r => r.GetAllAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            mocker.GetMock<INotifier>().Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Get contact by id with exception")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_GetByIdContactWithException_ShouldFailed()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryContact>()
                  .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                  .Throws(new Exception());
            var serviceContact = mocker.CreateInstance<ServiceContact>();

            //Act
            var contact = await serviceContact.GetByIdAsync(Guid.NewGuid());

            //Assert
            contact.Should().BeNull();
            mocker.GetMock<IRepositoryContact>()
                  .Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<INotifier>()
                  .Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
        }

        #endregion

        #region Função Remover

        [Fact(DisplayName = "Remove contact with success")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_RemoveContact_ShouldHasSuccess()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeEmail();
            var mocker = new AutoMocker();
            var serviceContact = mocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.RemoveAsyncUow(contact);

            //Assert
            mocker.GetMock<IRepositoryContact>()
                  .Verify(r => r.Update(It.IsAny<Contact>()), Times.Once);
            mocker.GetMock<IUnitOfWork>()
                  .Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Remove contact with exception")]
        [Trait("Categoria", "Contato - Serviço")]
        public async Task ContactService_RemoveContactWithException_ShouldFailed()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeEmail();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryContact>()
                  .Setup(r => r.Update(It.IsAny<Contact>()))
                  .Throws(new Exception());
            var serviceContact = mocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.RemoveAsyncUow(contact);

            //Assert
            mocker.GetMock<IRepositoryContact>()
                  .Verify(r => r.Update(It.IsAny<Contact>()), Times.Once);
            mocker.GetMock<IUnitOfWork>()
                  .Verify(r => r.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>()
                  .Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
            Assert.Throws<Exception>(() => serviceContact.Update(contact));
        }

        #endregion
    }
}