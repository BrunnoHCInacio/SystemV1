using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        #region Função Adicionar

        [Fact(DisplayName = "Add country with success")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_AddNewCountry_ShouldHaveSuccess()
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

        [Fact(DisplayName = "Add country with state with success")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_AddNewCountryWithState_ShouldHaveSuccess()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountryWithStates();
            var mocker = new AutoMocker();
            mocker.GetMock<IUnitOfWork>().Setup(u => u.CommitAsync()).Returns(Task.FromResult(true));
            var serviceCountry = mocker.CreateInstance<ServiceCountry>();

            //Act
            await serviceCountry.AddAsyncUow(country);

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.Add(It.IsAny<Country>()), Times.Once);
            mocker.GetMock<IRepositoryState>().Verify(r => r.Add(It.IsAny<State>()), Times.Exactly(country.States.Count));
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact( DisplayName ="Add country with exception")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_AddCountryWithException_ShouldFailed()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountry();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryCountry>().Setup(r => r.Add(It.IsAny<Country>())).Throws(new Exception());
            var countryService = mocker.CreateInstance<ServiceCountry>();

            //Act
            await countryService.AddAsyncUow(country);

            //Assert
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            Assert.Throws<Exception>(() => countryService.Add(country));
        }

        [Fact(DisplayName ="Add country with states with exception to add state")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_AddCountryWithStateExpection_ShoudFailed()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountryWithStates();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryState>().Setup(r => r.Add(It.IsAny<State>())).Throws(new Exception());
            var countryService = mocker.CreateInstance<ServiceCountry>();

            //Act
            await countryService.AddAsyncUow(country);

            //Assert
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<IServiceCountry>().Verify(c => c.Add(country), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Exactly(2));
        }
        
        [Fact(DisplayName = "Add country with fail")]
        [Trait("Categoria", "País - Serviço")]
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
        [Trait("Categoria", "País - Serviço")]
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
        [Trait("Categoria", "País - Serviço")]
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

        #endregion

        #region Função Modificar

        [Fact(DisplayName = "Update valid country with success")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_UpdateValidCountry_ShoudHasSuccess()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountry();
            var mocker = new AutoMocker();
            var countryService = mocker.CreateInstance<ServiceCountry>();

            //Act
            await countryService.UpdateAsyncUow(country);

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.Update(country), Times.Once);
            mocker.GetMock<IRepositoryState>().Verify(r => r.Update(It.IsAny<State>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName ="Update invalid country with fail")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_UpdateInvalidCountry_ShoulFailed()
        {
            //Arrange
            var country = _countryTestFixture.GenerateInvalidCountry();
            var mocker = new AutoMocker();
            var serviceCountry = mocker.CreateInstance<ServiceCountry>();

            //Act
            await serviceCountry.UpdateAsyncUow(country);

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.Update(It.IsAny<Country>()), Times.Never);
            mocker.GetMock<IRepositoryState>().Verify(r => r.Update(It.IsAny<State>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName ="Update valid country with valid state")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_UpdateCountryWithState_ShoulHasSuccess()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountryWithStates();
            var mocker = new AutoMocker();
            var serviceCountry = mocker.CreateInstance<ServiceCountry>();

            //Act
            await serviceCountry.UpdateAsyncUow(country);

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.Update(It.IsAny<Country>()), Times.Once);
            mocker.GetMock<IRepositoryState>().Verify(r => r.Update(It.IsAny<State>()), Times.Exactly(country.States.Count));
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName ="Update valid country with invalid states")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_UpdateValidCountryWithInvalidState_ShoulFailed()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountryWithInvalidStates();
            var mocker = new AutoMocker();
            var serviceCountry = mocker.CreateInstance<ServiceCountry>();

            //Act
            await serviceCountry.UpdateAsyncUow(country);

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.Update(It.IsAny<Country>()), Times.Never);
            mocker.GetMock<IRepositoryState>().Verify(r => r.Update(It.IsAny<State>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName ="Update valid country with exception")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_UpdateValidCountryWithException_ShoulFailed()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountry();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryCountry>().Setup(r => r.Update(It.IsAny<Country>())).Throws(new Exception());

            var serviceCountry = mocker.CreateInstance<ServiceCountry>();

            //Act
            await serviceCountry.UpdateAsyncUow(country);

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.Update(It.IsAny<Country>()), Times.Once);
            mocker.GetMock<IRepositoryState>().Verify(r => r.Update(It.IsAny<State>()), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(u => u.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Update valid country with states with exception update states")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_UpdateValidCountryStatesWithException_ShoulFailed()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountryWithStates();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryState>().Setup(r => r.Update(It.IsAny<State>())).Throws(new Exception());
            var serviceCountry = mocker.CreateInstance<ServiceCountry>();

            //Act
            await serviceCountry.UpdateAsyncUow(country);

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.Update(It.IsAny<Country>()), Times.Never);
            mocker.GetMock<IRepositoryState>().Verify(r => r.Update(It.IsAny<State>()), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(u => u.Handle(It.IsAny<Notification>()), Times.Once);
        }
        #endregion

        #region Função Obter
        
        [Fact(DisplayName ="Get all countries with success")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_GetAllCountries_ShouldHasSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryCountry>()
                  .Setup(r => r.GetAllCountriesAsync(It.IsAny<int>(), It.IsAny<int>()))
                  .Returns(Task.FromResult((IEnumerable<Country>)_countryTestFixture.GenerateCountry(4)));
            var countryService = mocker.CreateInstance<ServiceCountry>();

            //Act
            var countries = await countryService.GetAllAsync(1, 1);

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.GetAllCountriesAsync(1, 1),Times.Once);
            Assert.True(countries.Any());
        }

        [Fact(DisplayName = "Get all countries with exception")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_GetAllCountriesWithException_ShouldFailed()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryCountry>()
                  .Setup(r => r.GetAllCountriesAsync(It.IsAny<int>(), It.IsAny<int>()))
                  .Throws(new Exception());
            var countryService = mocker.CreateInstance<ServiceCountry>();

            //Act
            var countries = await countryService.GetAllAsync(1, 1);

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.GetAllCountriesAsync(1, 1), Times.Once);
            mocker.GetMock<INotifier>().Verify(r => r.Handle(It.IsAny<Notification>()), Times.Once);
            countries.Should().BeNull();
        }

        [Fact(DisplayName ="Get country by id with success")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_GetCountryById_ShouldHasSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryCountry>()
                  .Setup(r => r.GetCountryByIdAsync(It.IsAny<Guid>()))
                  .Returns(Task.FromResult(_countryTestFixture.GenerateValidCountry()));
            var countryService = mocker.CreateInstance<ServiceCountry>();

            //Act
            var country = await countryService.GetByIdAsync(Guid.NewGuid());

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.GetCountryByIdAsync(It.IsAny<Guid>()), Times.Once);
            country.Should().NotBeNull();
        }

        [Fact(DisplayName = "Get country by id with exception")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_GetCountryByIdWithException_ShouldFailed()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryCountry>()
                  .Setup(r => r.GetCountryByIdAsync(It.IsAny<Guid>()))
                  .Throws(new Exception());
            var countryService = mocker.CreateInstance<ServiceCountry>();

            //Act
            var country = await countryService.GetByIdAsync(Guid.NewGuid());

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.GetCountryByIdAsync(It.IsAny<Guid>()), Times.Once);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            country.Should().BeNull();
        }

        [Fact(DisplayName ="Get countries by name with success")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_GetCountriesByName_ShouldHasSuccess()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryCountry>()
                  .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                  .Returns(Task.FromResult((IEnumerable<Country>)_countryTestFixture.GenerateCountry(4)));
            var countryService = mocker.CreateInstance<ServiceCountry>();

            //Act
            var countries = await countryService.GetByNameAsync("teste");

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.GetByNameAsync(It.IsAny<string>()), Times.Once);
            Assert.True(countries.Any());
        }

        [Fact(DisplayName = "Get countries by name with exception")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_GetCountriesByNameWithException_ShouldFailed()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryCountry>()
                  .Setup(r => r.GetByNameAsync(It.IsAny<string>()))
                  .Throws(new Exception());
            var countryService = mocker.CreateInstance<ServiceCountry>();

            //Act
            var countries = await countryService.GetByNameAsync("teste");

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.GetByNameAsync(It.IsAny<string>()), Times.Once);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            countries.Should().BeNull();
        }

        [Fact(DisplayName = "Get countries by name with invalid name")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_GetCountriesByNameWithInvalidName_ShouldFailed()
        {
            //Arrange
            var mocker = new AutoMocker();
            var countryService = mocker.CreateInstance<ServiceCountry>();

            //Act
            var countries = await countryService.GetByNameAsync("");

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.GetByNameAsync(It.IsAny<string>()), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            countries.Should().BeNull();
        }

        #endregion

        #region Função Remover

        [Fact(DisplayName = "Remove country with success")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_RemoveCountry_ShoudHasSuccess()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountry();
            var mocker = new AutoMocker();
            var countryService = mocker.CreateInstance<ServiceCountry>();

            //Act
            await countryService.RemoveAsyncUow(country);

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.Update(country), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Remove country with exception")]
        [Trait("Categoria", "País - Serviço")]
        public async Task CountryService_RemoveCountryWithException_ShoudFailed()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountry();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryCountry>()
                  .Setup(r => r.Update(It.IsAny<Country>()))
                  .Throws(new Exception());
            var countryService = mocker.CreateInstance<ServiceCountry>();

            //Act
            await countryService.RemoveAsyncUow(country);

            //Assert
            mocker.GetMock<IRepositoryCountry>().Verify(r => r.Update(country), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            Assert.Throws<Exception>(() => countryService.Remove(country));
        }

        #endregion
    }
}