using FluentAssertions;
using Moq;
using Moq.AutoMock;
using System;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services;
using SystemV1.Domain.Services.Validations;
using SystemV1.Domain.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.ServiceTests
{
    [Collection(nameof(CityCollection))]
    public class CityServiceTest : ServiceTestBase
    {
        //Adicionar teste para exclusão de cidade de um endereço

        private readonly CityTestFixture _cityTestFixture;

        public CityServiceTest(CityTestFixture cityTestFixture) : base(new AutoMocker())
        {
            _cityTestFixture = cityTestFixture;
        }

        #region Function Add

        [Fact(DisplayName = "Add new valid city")]
        [Trait("UnitTests - Services", "City")]
        public async Task Add_AddNewValidCity_MustAddCityWithSuccess()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();
            SetSetupMock<City, IValidationCity>(city);
            var cityService = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.AddAsyncUow(city);

            //Assert
            _autoMocker.GetMock<IRepositoryCity>().Verify(r => r.Add(city), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add new invalid city")]
        [Trait("UnitTests - Services", "City")]
        public async Task Add_AddNewInvalidCity_MustNotAddCity()
        {
            //Arrange
            var city = _cityTestFixture.GenerateInvalidCity();
            var errors = GenerateMockErrors("Name", ValidationCity.NameRequired);
            SetSetupMock<City, IValidationCity>(city, errors);
            var cityService = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.AddAsyncUow(city);

            //Assert
            _autoMocker.GetMock<IRepositoryCity>().Verify(r => r.Add(city), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        }

        #endregion Function Add

        #region Function Update

        [Fact(DisplayName = "Update valid city")]
        [Trait("UnitTests - Services", "City")]
        public async Task Update_UpdateValidCity_MustUpdateCityWithSuccess()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();

            SetSetupMock<City, IValidationCity>(city);
            var cityService = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.UpdateAsyncUow(city);

            //Assert
            _autoMocker.GetMock<IRepositoryCity>().Verify(r => r.Update(city), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update invalid city")]
        [Trait("UnitTests - Services", "City")]
        public async Task Update_UpdateInvalidCity_MustNotUpdateCity()
        {
            //Arrange
            var city = _cityTestFixture.GenerateInvalidCity();
            var errors = GenerateMockErrors("Name", ValidationCity.NameRequired);
            SetSetupMock<City, IValidationCity>(city, errors);
            var cityService = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.UpdateAsyncUow(city);

            //Assert
            _autoMocker.GetMock<IRepositoryCity>().Verify(r => r.Add(city), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        }

        #endregion Function Update

        #region Function Remove

        [Fact(DisplayName = "Remove city with success")]
        [Trait("UnitTests - Services", "City")]
        public async Task Remove_RemoveCity_MustRemoveCityWithSuccess()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();

            SetSetupMock<City, IValidationCity>(city);
            var cityService = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.RemoveAsyncUow(city);

            //Assert
            _autoMocker.GetMock<IRepositoryCity>().Verify(r => r.Remove(city), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Remove city with references")]
        [Trait("UnitTests - Services", "City")]
        public async Task Remove_RemoveCityWithReferences_ShouldNotRemove()
        {
            //Arrange
            var city = _cityTestFixture.GenerateValidCity();
            var errors = GenerateMockErrors("", ValidationCity.CityNotDeleteHaveLinks);
            SetSetupMock<City, IValidationCity>(city, errors);

            var cityService = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            await cityService.RemoveAsyncUow(city);

            //Assert
            _autoMocker.GetMock<IRepositoryCity>().Verify(r => r.Remove(city), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        }

        #endregion Function Remove

        #region Function Get

        [Fact(DisplayName = "Get all citeis")]
        [Trait("UnitTests - Services", "City")]
        public async Task GetAll_GetAllCities_MustReturnListOfTheCity()
        {
            //Arrange
            var page = 1;
            var pageSize = 10;
            _autoMocker.GetMock<IRepositoryCity>()
                  .Setup(r => r.SearchAsync(null, page, pageSize))
                  .Returns(Task.FromResult(_cityTestFixture.GenerateCity(5)));
            var serviceCity = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            var cities = await serviceCity.SearchAsync(null, page, pageSize);

            //Assert
            Assert.NotEmpty(cities);
        }

        [Fact(DisplayName = "Get city by id with success")]
        [Trait("UnitTests - Services", "City")]
        public async Task GetById_GetCityByIdAsync_MustReturnACity()
        {
            //Arrange
            var cityId = Guid.NewGuid();
            _autoMocker.GetMock<IRepositoryCity>()
                       .Setup(r => r.GetEntityAsync(c => c.Id == cityId, null))
                       .Returns(Task.FromResult(_cityTestFixture.GenerateValidCity()));
            var serviceCity = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            var city = await serviceCity.GetEntityAsync(c => c.Id == cityId, null);

            //Assert
            Assert.NotNull(city);
        }

        [Fact(DisplayName = "Get city by name with success")]
        [Trait("UnitTests - Services", "City")]
        public async Task GetByName_GetByName_MustReturnListOfTheCities()
        {
            //Arrange
            var name = "aaaa";
            _autoMocker.GetMock<IRepositoryCity>()
                       .Setup(r => r.SearchAsync(c => c.Name.ToUpper() == name.ToUpper()))
                       .Returns(Task.FromResult(_cityTestFixture.GenerateCity(10)));
            var serviceCity = _autoMocker.CreateInstance<ServiceCity>();

            //Act
            var cities = await serviceCity.SearchAsync(c => c.Name.ToUpper() == name.ToUpper());

            //Assert

            cities.Should().NotBeEmpty();
            cities.Should().HaveCount(10);
        }

        #endregion Function Get
    }
}