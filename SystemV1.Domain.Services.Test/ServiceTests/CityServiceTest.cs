using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
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
    [Collection(nameof(CityCollection))]
    public class CityServiceTest
    {
        private readonly CityTestFixture _cityTestFixture;
        private readonly AutoMocker _autoMocker;

        public CityServiceTest(CityTestFixture cityTestFixture)
        {
            _cityTestFixture = cityTestFixture;
            _autoMocker = new AutoMocker();
        }

        #region Function Add

        [Fact(DisplayName = "Add new valid city")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task Add_AddNewValidCity_MustAddCityWithSuccess()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();
            var cityService = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.AddAsyncUow(city);

            //Assert
            _autoMocker.GetMock<IRepositoryCity>().Verify(r => r.Add(city), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add new invalid city")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task Add_AddNewInvalidCity_MustNotAddCity()
        {
            //Arrange
            var city = _cityTestFixture.GenerateInvalidCity();
            var cityService = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.AddAsyncUow(city);

            //Assert
            _autoMocker.GetMock<IRepositoryCity>().Verify(r => r.Add(city), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Add new valid city with exception on repository")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task Add_AddNewValidCityWithExceptionOnRepository_MustNotAddCityAndNotifyErrors()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();
            _autoMocker.GetMock<IRepositoryCity>().Setup(r => r.Add(It.IsAny<City>())).Throws(new Exception());
            var cityService = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.AddAsyncUow(city);

            //Assert
            _autoMocker.GetMock<IRepositoryCity>().Verify(r => r.Add(city), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            Assert.Throws<Exception>(() => cityService.Add(city));
        }
        #endregion

        #region Function Update
        [Fact(DisplayName = "Update valid city")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task Update_UpdateValidCity_MustUpdateCityWithSuccess()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();
            var cityService = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.UpdateAsyncUow(city);

            //Assert
            _autoMocker.GetMock<IRepositoryCity>().Verify(r => r.Update(city), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update invalid city")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task Update_UpdateInvalidCity_MustNotUpdateCity()
        {
            //Arrange
            var city = _cityTestFixture.GenerateInvalidCity();
            var cityService = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.UpdateAsyncUow(city);

            //Assert
            _autoMocker.GetMock<IRepositoryCity>().Verify(r => r.Add(city), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Update valid city with exception on repository")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task Update_UptadeValidCityWithExceptionOnRepository_MustNotUPdateCityAndNotifyErrors()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();
            _autoMocker.GetMock<IRepositoryCity>().Setup(r => r.Update(It.IsAny<City>())).Throws(new Exception());
            var cityService = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.UpdateAsyncUow(city);

            //Assert
            _autoMocker.GetMock<IRepositoryCity>().Verify(r => r.Update(city), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            Assert.Throws<Exception>(() => cityService.Update(city));
        }
        #endregion

        #region Function Remove
        [Fact(DisplayName = "Remove city with success")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task Remove_RemoveCity_MustRemoveCityWithSuccess()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();
            var cityService = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.RemoveAsyncUow(city);

            //Assert
            _autoMocker.GetMock<IRepositoryCity>().Verify(r => r.Update(city), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Remove city with exception on repository")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task Remove_RemoveCityWithExceptionOnRepository_MustNotRemoveCityAndNotifyErrors()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();
            _autoMocker.GetMock<IRepositoryCity>().Setup(r => r.Update(It.IsAny<City>())).Throws(new Exception());
            var cityService = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.RemoveAsyncUow(city);

            //Assert
            _autoMocker.GetMock<IRepositoryCity>().Verify(r => r.Update(city), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            Assert.Throws<Exception>(() => cityService.Remove(city));
        }
        #endregion

        #region Function Get
        [Fact(DisplayName = "Get all citeis")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task GetAll_GetAllCities_MustReturnListOfTheCity()
        {
            //Arrange
            _autoMocker.GetMock<IRepositoryCity>()
                  .Setup(r => r.GetAllCitiesAsync(1, 10))
                  .Returns(Task.FromResult((IEnumerable<City>)_cityTestFixture.GenerateCity(5)));
            var serviceCity = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            var cities = await serviceCity.GetAllAsync(1, 10);

            //Assert
            Assert.NotEmpty(cities);
        }

        [Fact(DisplayName = "Get all citeis with exception")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task GetAll_GetAllCities_MustReturnErrorNotification()
        {
            //Arrange
            _autoMocker.GetMock<IRepositoryCity>()
                  .Setup(r => r.GetAllCitiesAsync(1, 10))
                  .Throws(new Exception());
            var serviceCity = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            var cities = await serviceCity.GetAllAsync(1, 10);

            //Assert
            Assert.Null(cities);
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Get city by id with success")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task GetById_GetCityByIdAsync_MustReturnACity()
        {
            //Arrange
            _autoMocker.GetMock<IRepositoryCity>()
                       .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                       .Returns(Task.FromResult(_cityTestFixture.GenerateValidCity()));
            var serviceCity = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            var city = await serviceCity.GetByIdAsync(Guid.NewGuid());

            //Assert
            Assert.NotNull(city);
        }

        [Fact(DisplayName = "Get city by id with exception")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task GetById_GetCityByIdAsync_MustReturnErrorNotification()
        {
            //Arrange
            _autoMocker.GetMock<IRepositoryCity>()
                       .Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                       .Throws(new Exception());
            var serviceCity = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            var city = await serviceCity.GetByIdAsync(Guid.NewGuid());

            //Assert
            Assert.Null(city);
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName ="Get city by name with success")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task GetByName_GetByName_MustReturnListOfTheCities()
        {
            //Arrange
            _autoMocker.GetMock<IRepositoryCity>()
                       .Setup(r => r.GetByNameAsync("AAAss"))
                       .Returns(Task.FromResult((IEnumerable<City>)_cityTestFixture.GenerateCity(10)));
            var serviceCity = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            var cities = await serviceCity.GetByNameAsync("AAAss");

            //Assert

            Assert.NotEmpty(cities);
        }

        [Fact(DisplayName = "Get city by invalid name")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task GetByName_GetByNameWithInvalidName_MustReturndNotifyMessage()
        {
            //Arrange
            var serviceCity = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            var cities = await serviceCity.GetByNameAsync("");

            //Assert

            Assert.Null(cities);
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }

        [Fact(DisplayName = "Get city by name with exception")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task GetByName_GetByNameWithException_MustReturndNotifyMessage()
        {
            //Arrange
            _autoMocker.GetMock<IRepositoryCity>()
                       .Setup(r => r.GetByNameAsync("AAA"))
                       .Throws(new Exception());
            var serviceCity = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            var cities = await serviceCity.GetByNameAsync("AAA");

            //Assert

            Assert.Null(cities);
            _autoMocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
        }
        #endregion
    }
}
