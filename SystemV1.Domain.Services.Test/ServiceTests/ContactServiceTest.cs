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
using SystemV1.Domain.Enums;
using SystemV1.Domain.Services;
using SystemV1.Domain.Services.Notifications;
using SystemV1.Domain.Services.Validations;
using SystemV1.Domain.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.ServiceTests
{
    [Collection(nameof(ContactCollection))]
    public class ContactServiceTest : ServiceTestBase
    {
        private readonly ContactTestFixture _contactTestFixture;

        public ContactServiceTest(ContactTestFixture contactTestFixture) : base(new AutoMocker())
        {
            _contactTestFixture = contactTestFixture;
        }

        #region Função Adicionar

        [Fact(DisplayName = "Add contact email with success")]
        [Trait("UnitTests - Services", "Contact")]
        public async Task ContactService_NewContactTypeEmail_ShouldHaveSuccess()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeEmail();
            SetSetupMock<Contact, IValidationContact>(contact);
            var serviceContact = _autoMocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.AddAsyncUow(contact);

            //Assert
            _autoMocker.GetMock<IRepositoryContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add contact phone with success")]
        [Trait("UnitTests - Services", "Contact")]
        public async Task ContactService_NewContactTypePhone_ShouldHaveSuccess()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypePhone();
            SetSetupMock<Contact, IValidationContact>(contact);
            var serviceContact = _autoMocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.AddAsyncUow(contact);

            //Assert
            _autoMocker.GetMock<IRepositoryContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add contact cell phone with success")]
        [Trait("UnitTests - Services", "Contact")]
        public async Task ContactService_NewContactTypeCellPhone_ShouldHaveSuccess()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeCellPhone();
            SetSetupMock<Contact, IValidationContact>(contact);
            var serviceContact = _autoMocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.AddAsyncUow(contact);

            //Assert
            _autoMocker.GetMock<IRepositoryContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add contact email with fail")]
        [Trait("UnitTests - Services", "Contact")]
        public async Task ContactService_NewContactTypeEmail_ShouldHaveFail()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateInvalidEmailContactWithInvalidProperies();
            var errors = GenerateMockErrors("TypeContact", ValidationContact.TypeContactRequired);
            SetSetupMock<Contact, IValidationContact>(contact, errors);
            var serviceContact = _autoMocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.AddAsyncUow(contact);

            //Assert
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
            _autoMocker.GetMock<IRepositoryContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Add contact phone with fail")]
        [Trait("UnitTests - Services", "Contact")]
        public async Task ContactService_NewContactTypePhone_ShouldHaveFail()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateInvalidPhoneContactWithInvalidProperties();
            var errors = GenerateMockErrors("TypeContact", ValidationContact.TypeContactRequired);
            SetSetupMock<Contact, IValidationContact>(contact, errors);
            var serviceContact = _autoMocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.AddAsyncUow(contact);

            //Assert
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
            _autoMocker.GetMock<IRepositoryContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Add contact cell phone with fail")]
        [Trait("UnitTests - Services", "Contact")]
        public async Task ContactService_NewContactTypeCellPhone_ShouldHaveFail()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateInvalidCellPhoneContactWithInvalidProperies();
            var errors = GenerateMockErrors("TypeContact", ValidationContact.TypeContactRequired);
            SetSetupMock<Contact, IValidationContact>(contact, errors);
            var serviceContact = _autoMocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.AddAsyncUow(contact);

            //Assert
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
            _autoMocker.GetMock<IRepositoryContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }

        #endregion Função Adicionar

        #region Função Modificar

        [Fact(DisplayName = "Update contact email with success")]
        [Trait("UnitTests - Services", "Contact")]
        public async Task ContactService_UpdateContactTypeEmail_ShouldHaveSuccess()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeEmail();
            SetSetupMock<Contact, IValidationContact>(contact);
            var serviceContact = _autoMocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.UpdateAsyncUow(contact);

            //Assert
            _autoMocker.GetMock<IRepositoryContact>().Verify(r => r.Update(It.IsAny<Contact>()), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update contact phone with success")]
        [Trait("UnitTests - Services", "Contact")]
        public async Task ContactService_UpdateContactTypePhone_ShouldHaveSuccess()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypePhone();

            SetSetupMock<Contact, IValidationContact>(contact);
            var serviceContact = _autoMocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.UpdateAsyncUow(contact);

            //Assert
            _autoMocker.GetMock<IRepositoryContact>().Verify(r => r.Update(It.IsAny<Contact>()), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update contact cell phone with success")]
        [Trait("UnitTests - Services", "Contact")]
        public async Task ContactService_UpdateContactTypeCellPhone_ShouldHaveSuccess()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeCellPhone();

            SetSetupMock<Contact, IValidationContact>(contact);
            var serviceContact = _autoMocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.UpdateAsyncUow(contact);

            //Assert
            _autoMocker.GetMock<IRepositoryContact>().Verify(r => r.Update(It.IsAny<Contact>()), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update contact email with fail")]
        [Trait("UnitTests - Services", "Contact")]
        public async Task ContactService_NewContactTypeEmail_ShouldFailed()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateInvalidEmailContactWithInvalidProperies();
            var errors = GenerateMockErrors("TypeContact", ValidationContact.TypeContactRequired);
            SetSetupMock<Contact, IValidationContact>(contact, errors);
            var serviceContact = _autoMocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.UpdateAsyncUow(contact);

            //Assert
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
            _autoMocker.GetMock<IRepositoryContact>().Verify(r => r.Update(It.IsAny<Contact>()), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Update contact phone with fail")]
        [Trait("UnitTests - Services", "Contact")]
        public async Task ContactService_UpdateContactTypePhone_ShouldFailed()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateInvalidPhoneContactWithInvalidProperties();
            var errors = GenerateMockErrors("TypeContact", ValidationContact.TypeContactRequired);
            SetSetupMock<Contact, IValidationContact>(contact, errors);
            var serviceContact = _autoMocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.UpdateAsyncUow(contact);

            //Assert
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
            _autoMocker.GetMock<IRepositoryContact>().Verify(r => r.Update(It.IsAny<Contact>()), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Update contact cell phone with fail")]
        [Trait("UnitTests - Services", "Contact")]
        public async Task ContactService_UpdateContactTypeCellPhone_ShouldFailed()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateInvalidCellPhoneContactWithInvalidProperies();
            var errors = GenerateMockErrors("TypeContact", ValidationContact.TypeContactRequired);
            SetSetupMock<Contact, IValidationContact>(contact, errors);
            var serviceContact = _autoMocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.UpdateAsyncUow(contact);

            //Assert
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()));
            _autoMocker.GetMock<IRepositoryContact>().Verify(r => r.Update(It.IsAny<Contact>()), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }

        #endregion Função Modificar

        #region Função Obter

        [Fact(DisplayName = "Get all contacts with success")]
        [Trait("UnitTests - Services", "Contact")]
        public async Task ContactService_GetAllContacts_ShouldHasSuccess()
        {
            //Arrange
            var page = 1;
            var pageSize = 10;
            _autoMocker.GetMock<IRepositoryContact>()
                  .Setup(r => r.SearchAsync(null, page, pageSize))
                  .Returns(Task.FromResult(_contactTestFixture.GenerateContact(EnumTypeContact.TypeContactPhone, 5)));
            var serviceContact = _autoMocker.CreateInstance<ServiceContact>();

            //Act
            var contacts = await serviceContact.SearchAsync(null, page, pageSize);

            //Assert
            contacts.Should().NotBeEmpty();
            Assert.Equal(5, contacts.Count());
            _autoMocker.GetMock<IRepositoryContact>().Verify(r => r.SearchAsync(null, page, pageSize), Times.Once);
        }

        [Fact(DisplayName = "Get contact by id with success")]
        [Trait("UnitTests - Services", "Contact")]
        public async Task ContactService_GetContactById_ShouldHasSuccess()
        {
            //Arrange
            var contactId = Guid.NewGuid();
            _autoMocker.GetMock<IRepositoryContact>()
                  .Setup(r => r.GetEntityAsync(c => c.Id == contactId, null))
                  .Returns(Task.FromResult(_contactTestFixture.GenerateValidContactTypeCellPhone()));
            var serviceContact = _autoMocker.CreateInstance<ServiceContact>();

            //Act
            var contact = await serviceContact.GetEntityAsync(c => c.Id == contactId, null);

            //Assert
            contact.Should().NotBeNull();
            _autoMocker.GetMock<IRepositoryContact>().Verify(r => r.GetEntityAsync(c => c.Id == contactId, null), Times.Once);
        }

        #endregion Função Obter

        #region Função Remover

        [Fact(DisplayName = "Remove contact with success")]
        [Trait("UnitTests - Services", "Contact")]
        public async Task ContactService_RemoveContact_ShouldHasSuccess()
        {
            //Arrange
            var contact = _contactTestFixture.GenerateValidContactTypeEmail();
            SetSetupMock<Contact, IValidationContact>(contact);
            var serviceContact = _autoMocker.CreateInstance<ServiceContact>();

            //Act
            await serviceContact.RemoveAsyncUow(contact);

            //Assert
            _autoMocker.GetMock<IRepositoryContact>()
                  .Verify(r => r.Remove(It.IsAny<Contact>()), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>()
                  .Verify(r => r.CommitAsync(), Times.Once);
        }

        #endregion Função Remover
    }
}