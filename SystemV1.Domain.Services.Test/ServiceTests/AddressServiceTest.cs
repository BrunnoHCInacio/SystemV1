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
    [Collection(nameof(AddressCollection))]
    public class AddressServiceTest
    {
        private readonly AddressTestFixture _addressTestFixture;

        public AddressServiceTest(AddressTestFixture addressTestFixture)
        {
            _addressTestFixture = addressTestFixture;
        }

        [Fact(DisplayName = "Add new address with success")]
        [Trait("Categoria", "Serviço - Endereço")]
        public async Task AddressService_NewAddress_ShouldBeSuccess()
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
        [Trait("Categoria", "Serviço - Endereço")]
        public async Task AddressService_NewAddress_ShouldBeFail()
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
            mocker.GetMock<IRepositoryContact>().Verify(r => r.Add(It.IsAny<Contact>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(r => r.CommitAsync(), Times.Never);
        }
    }
}