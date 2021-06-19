using FluentValidation.Results;
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
    [Collection(nameof(CountryCollection))]
    public class CountryServiceTest
    {
        private readonly CountryTestFixture _countryTestFixture;

        public CountryServiceTest(CountryTestFixture countryTestFixture)
        {
            _countryTestFixture = countryTestFixture;
        }

        [Fact(DisplayName = "Add country with success")]
        [Trait("Categoria", "Serviço - País")]
        public async Task CountryService_NewCountry_ShouldHaveSuccess()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountry();
            var mocker = new AutoMocker();
            var serviceCountry = mocker.CreateInstance<ServiceCountry>();
            mocker.GetMock<IUnitOfWork>().Setup(u => u.CommitAsync()).Returns(Task.FromResult(true));

            //Act
            await serviceCountry.AddAsyncUow(country);

            //Asert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.Add(It.IsAny<Country>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add country with state should have success")]
        [Trait("Categoria", "Serviço - País")]
        public async Task CountryService_NewCountryWithState_ShouldHaveSuccess()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountryWithStates();
            var mocker = new AutoMocker();
            var serviceCountry = mocker.CreateInstance<ServiceCountry>();
            mocker.GetMock<IUnitOfWork>().Setup(u => u.CommitAsync()).Returns(Task.FromResult(true));

            //Act
            await serviceCountry.AddAsyncUow(country);

            //Asert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.Add(It.IsAny<Country>()), Times.Once);
            mocker.GetMock<IRepositoryState>().Verify(r => r.Add(It.IsAny<State>()), Times.Exactly(country.States.Count));
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add country with fail")]
        [Trait("Categoria", "Serviço - País")]
        public async Task CountryService_NewCountry_ShouldHaveFail()
        {
            //Arrange
            var country = _countryTestFixture.GenerateInvalidCountry();
            var mocker = new AutoMocker();
            var serviceCountry = mocker.CreateInstance<ServiceCountry>();

            //Act
            await serviceCountry.AddAsyncUow(country);

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.Add(It.IsAny<Country>()), Times.Never);
            mocker.GetMock<IRepositoryState>().Verify(r => r.Add(It.IsAny<State>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Add country with state should be fail")]
        [Trait("Categoria", "Serviço - País")]
        public async Task CountryService_NewCountryWithState_ShouldBeFail()
        {
            //Arrange
            var country = _countryTestFixture.GenerateInvalidCountryWithStates();
            var mocker = new AutoMocker();
            var serviceCountry = mocker.CreateInstance<ServiceCountry>();

            //Act
            await serviceCountry.AddAsyncUow(country);

            //Asert
            mocker.GetMock<INotifier>()
                  .Verify(n => n.Handle(It.IsAny<Notification>()));

            mocker.GetMock<IRepositoryCountry>()
                  .Verify(r => r.Add(It.IsAny<Country>()),
                               Times.Never);

            mocker.GetMock<IRepositoryState>()
               .Verify(r => r.Add(It.IsAny<State>()),
                            Times.Never);

            mocker.GetMock<IUnitOfWork>()
                  .Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Add valid country with invalid state should be fail")]
        [Trait("Categoria", "Serviço - País")]
        public async Task CountryService_NewValidCountryWithInvalidState_ShouldBeFail()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountryWithInvalidStates();
            var mocker = new AutoMocker();
            var serviceCountry = mocker.CreateInstance<ServiceCountry>();

            //Act
            await serviceCountry.AddAsyncUow(country);

            //Asert
            mocker.GetMock<INotifier>()
                  .Verify(n => n.Handle(It.IsAny<Notification>()));

            mocker.GetMock<IRepositoryCountry>()
                  .Verify(r => r.Add(It.IsAny<Country>()),
                               Times.Never);

            mocker.GetMock<IRepositoryState>()
               .Verify(r => r.Add(It.IsAny<State>()),
                            Times.Never);

            mocker.GetMock<IUnitOfWork>()
                  .Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}