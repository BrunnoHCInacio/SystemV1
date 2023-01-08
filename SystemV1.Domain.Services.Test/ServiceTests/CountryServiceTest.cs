using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
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
    [Collection(nameof(CountryCollection))]
    public class CountryServiceTest : ServiceTestBase
    {
        private readonly CountryTestFixture _countryTestFixture;

        public CountryServiceTest(CountryTestFixture countryTestFixture) : base(new AutoMocker())
        {
            _countryTestFixture = countryTestFixture;
        }

        #region Função Adicionar

        [Fact(DisplayName = "Add country with success")]
        [Trait("UnitTests - Services", "Country")]
        public async Task CountryService_AddNewCountry_ShouldHaveSuccess()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountry();
            var serviceCountry = _autoMocker.CreateInstance<ServiceCountry>();
            SetSetupMock<Country, IValidationCountry>(country);

            //Act
            await serviceCountry.AddAsyncUow(country);

            //Assert
            _autoMocker.GetMock<IRepositoryCountry>().Verify(r => r.Add(It.IsAny<Country>()), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Add country with fail")]
        [Trait("UnitTests - Services", "Country")]
        public async Task CountryService_NewCountry_ShouldFailed()
        {
            //Arrange
            var country = _countryTestFixture.GenerateInvalidCountry();
            var serviceCountry = _autoMocker.CreateInstance<ServiceCountry>();

            var errors = GenerateMockErrors("Name", ValidationCountry.CountryNameRequired);
            SetSetupMock<Country, IValidationCountry>(country, errors);

            //Act
            await serviceCountry.AddAsyncUow(country);

            //Assert
            _autoMocker.GetMock<IRepositoryCountry>().Verify(r => r.Add(It.IsAny<Country>()), Times.Never);
            _autoMocker.GetMock<IRepositoryState>().Verify(r => r.Add(It.IsAny<State>()), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        }

        #endregion Função Adicionar

        #region Função Modificar

        [Fact(DisplayName = "Update valid country with success")]
        [Trait("UnitTests - Services", "Country")]
        public async Task CountryService_UpdateValidCountry_ShoudHasSuccess()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountry();
            var countryService = _autoMocker.CreateInstance<ServiceCountry>();
            SetSetupMock<Country, IValidationCountry>(country);

            //Act
            await countryService.UpdateAsyncUow(country);

            //Assert
            _autoMocker.GetMock<IRepositoryCountry>().Verify(r => r.Update(country), Times.Once);
            _autoMocker.GetMock<IRepositoryState>().Verify(r => r.Update(It.IsAny<State>()), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact(DisplayName = "Update invalid country with fail")]
        [Trait("UnitTests - Services", "Country")]
        public async Task CountryService_UpdateInvalidCountry_ShoulFailed()
        {
            //Arrange
            var country = _countryTestFixture.GenerateInvalidCountry();

            var serviceCountry = _autoMocker.CreateInstance<ServiceCountry>();
            var errors = GenerateMockErrors("Name", ValidationCountry.CountryNameRequired);
            SetSetupMock<Country, IValidationCountry>(country, errors);

            //Act
            await serviceCountry.UpdateAsyncUow(country);

            //Assert
            _autoMocker.GetMock<IRepositoryCountry>().Verify(r => r.Update(It.IsAny<Country>()), Times.Never);
            _autoMocker.GetMock<IRepositoryState>().Verify(r => r.Update(It.IsAny<State>()), Times.Never);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Never);
        }

        #endregion Função Modificar

        #region Função Obter

        [Fact(DisplayName = "Get all countries with success")]
        [Trait("UnitTests - Services", "Country")]
        public async Task CountryService_GetAllCountries_ShouldHasSuccess()
        {
            //Arrange

            _autoMocker.GetMock<IRepositoryCountry>()
                  .Setup(r => r.SearchAsync(null, It.IsAny<int>(), It.IsAny<int>()))
                  .Returns(Task.FromResult(_countryTestFixture.GenerateCountry(4)));
            var countryService = _autoMocker.CreateInstance<ServiceCountry>();

            //Act
            var countries = await countryService.SearchAsync(null, It.IsAny<int>(), It.IsAny<int>());

            //Assert
            _autoMocker.GetMock<IRepositoryCountry>().Verify(r => r.SearchAsync(null, It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            Assert.True(countries.Any());
        }

        [Fact(DisplayName = "Get country by id with success")]
        [Trait("UnitTests - Services", "Country")]
        public async Task CountryService_GetCountryById_ShouldHasSuccess()
        {
            //Arrange

            var countryId = Guid.NewGuid();
            _autoMocker.GetMock<IRepositoryCountry>()
                  .Setup(r => r.GetEntityAsync(c => c.Id == countryId, null))
                  .Returns(Task.FromResult(_countryTestFixture.GenerateValidCountry()));
            var countryService = _autoMocker.CreateInstance<ServiceCountry>();

            //Act
            var country = await countryService.GetEntityAsync(c => c.Id == countryId, null);

            //Assert
            _autoMocker.GetMock<IRepositoryCountry>().Verify(r => r.GetEntityAsync(c => c.Id == countryId, null), Times.Once);
            country.Should().NotBeNull();
        }

        [Fact(DisplayName = "Get countries by name with success")]
        [Trait("UnitTests - Services", "Country")]
        public async Task CountryService_GetCountriesByName_ShouldHasSuccess()
        {
            //Arrange

            var name = "aaaa";
            _autoMocker.GetMock<IRepositoryCountry>()
                  .Setup(r => r.SearchAsync(c => c.Name.ToUpper() == name.ToUpper()))
                  .Returns(Task.FromResult(_countryTestFixture.GenerateCountry(4)));
            var countryService = _autoMocker.CreateInstance<ServiceCountry>();

            //Act
            var countries = await countryService.SearchAsync(c => c.Name.ToUpper() == name.ToUpper());

            //Assert
            _autoMocker.GetMock<IRepositoryCountry>().Verify(r => r.SearchAsync(c => c.Name.ToUpper() == name.ToUpper()), Times.Once);
            Assert.True(countries.Any());
        }

        [Fact(DisplayName = "Get countries by name with invalid name")]
        [Trait("UnitTests - Services", "Country")]
        public async Task CountryService_GetCountriesByNameWithInvalidName_ShouldFailed()
        {
            //Arrange

            var countryService = _autoMocker.CreateInstance<ServiceCountry>();

            //Act
            var countries = await countryService.SearchAsync(c => c.Name == "");

            //Assert
            countries.Should().BeNull();
        }

        #endregion Função Obter

        #region Função Remover

        [Fact(DisplayName = "Remove country with success")]
        [Trait("UnitTests - Services", "Country")]
        public async Task CountryService_RemoveCountry_ShoudHasSuccess()
        {
            //Arrange
            var country = _countryTestFixture.GenerateValidCountry();

            var countryService = _autoMocker.CreateInstance<ServiceCountry>();
            SetSetupMock<Country, IValidationCountry>(country);

            //Act
            await countryService.RemoveAsyncUow(country);

            //Assert
            _autoMocker.GetMock<IRepositoryCountry>().Verify(r => r.Remove(country), Times.Once);
            _autoMocker.GetMock<IUnitOfWork>().Verify(u => u.CommitAsync(), Times.Once);
        }

        #endregion Função Remover

        #region Function Exists Country

        [Fact(DisplayName = "Verify if exists register of the country")]
        [Trait("UnitTests - Services", "Country")]
        public async Task ExistsCountry_VerifyIfExistsCountryById_MustReturnTrue()
        {
            //Arrange

            _autoMocker.GetMock<IRepositoryCountry>()
                  .Setup(r => r.ExistsAsync(c => c.Id == Guid.Empty))
                  .Returns(Task.FromResult(true));

            var serviceCountry = _autoMocker.CreateInstance<ServiceCountry>();

            //Act
            var exist = await serviceCountry.ExistsAsync(c => c.Id == Guid.Empty);

            //Arrange
            Assert.True(exist);
        }

        #endregion Function Exists Country
    }
}