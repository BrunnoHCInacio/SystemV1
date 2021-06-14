using Moq;
using Moq.AutoMock;
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
    [Collection(nameof(ContactCollection))]
    public class ContactServiceTest
    {
        private readonly ContactTestFixture _contactTestFixture;

        public ContactServiceTest(ContactTestFixture contactTestFixture)
        {
            _contactTestFixture = contactTestFixture;
        }

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
    }
}