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

        public CityServiceTest(CityTestFixture cityTestFixture)
        {
            _cityTestFixture = cityTestFixture;
        }

        #region Function Add

        [Fact(DisplayName = "Add new valid city")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task Add_AddNewValidCity_MustAddCityWithSuccess()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();
            var mocker = new AutoMocker();
            var cityService = mocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.AddAsyncUow(city);

            //Assert
            mocker.GetMock<IRepositoryCity>().Verify(r => r.Add(city), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add new invalid city")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task Add_AddNewInvalidCity_MustNotAddCity()
        {
            //Arrange
            var city = _cityTestFixture.GenerateInvalidCity();
            var mocker = new AutoMocker();
            var cityService = mocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.AddAsyncUow(city);

            //Assert
            mocker.GetMock<IRepositoryCity>().Verify(r => r.Add(city), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Add new valid city with exception on repository")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task Add_AddNewValidCityWithExceptionOnRepository_MustNotAddCityAndNotifyErrors()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryCity>().Setup(r => r.Add(It.IsAny<City>())).Throws(new Exception());
            var cityService = mocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.AddAsyncUow(city);

            //Assert
            mocker.GetMock<IRepositoryCity>().Verify(r => r.Add(city), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
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
            var mocker = new AutoMocker();
            var cityService = mocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.UpdateAsyncUow(city);

            //Assert
            mocker.GetMock<IRepositoryCity>().Verify(r => r.Update(city), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update invalid city")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task Update_UpdateInvalidCity_MustNotUpdateCity()
        {
            //Arrange
            var city = _cityTestFixture.GenerateInvalidCity();
            var mocker = new AutoMocker();
            var cityService = mocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.UpdateAsyncUow(city);

            //Assert
            mocker.GetMock<IRepositoryCity>().Verify(r => r.Add(city), Times.Never);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        }

        [Fact(DisplayName = "Update valid city with exception on repository")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task Update_UptadeValidCityWithExceptionOnRepository_MustNotUPdateCityAndNotifyErrors()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryCity>().Setup(r => r.Update(It.IsAny<City>())).Throws(new Exception());
            var cityService = mocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.UpdateAsyncUow(city);

            //Assert
            mocker.GetMock<IRepositoryCity>().Verify(r => r.Update(city), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
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
            var mocker = new AutoMocker();
            var cityService = mocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.RemoveAsyncUow(city);

            //Assert
            mocker.GetMock<IRepositoryCity>().Verify(r => r.Update(city), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Remove city with exception on repository")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task Remove_RemoveCityWithExceptionOnRepository_MustNotRemoveCityAndNotifyErrors()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryCity>().Setup(r => r.Update(It.IsAny<City>())).Throws(new Exception());
            var cityService = mocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.RemoveAsyncUow(city);

            //Assert
            mocker.GetMock<IRepositoryCity>().Verify(r => r.Update(city), Times.Once);
            mocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
            mocker.GetMock<INotifier>().Verify(n => n.Handle(It.IsAny<Notification>()), Times.Once);
            Assert.Throws<Exception>(() => cityService.Remove(city));
        }
        #endregion

        [Fact(DisplayName = "Get all citeis")]
        [Trait("Categoria", "Cidade - Serviço")]
        public async Task GetAll_GetAllCities_MustReturnListOfTheCity()
        {
            //Arrange
            var mocker = new AutoMocker();
            mocker.GetMock<IRepositoryCity>()
                  .Setup(r => r.GetAllCities(1, 1))
                  .Returns(Task.FromResult((IEnumerable<City>)_cityTestFixture.Generate(5)));
            var serviceCity = mocker.CreateInstance<ServiceCity>();

            //Act
            var cities = await serviceCity.GetAllAsync(1, 10);

            //Assert
            Assert.NotEmpty(cities);
        }
    }
}
