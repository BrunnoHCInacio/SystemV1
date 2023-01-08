using API2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using SystemV1.Application.Resources;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Services.Test.Fixture;
using SystemV1.Domain.Test.Fixture;
using SystemV1.Domain.Test.IntegrationTest.Config;
using Xunit;
using Xunit.Priority;

namespace SystemV1.Domain.Test.IntegrationTest
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    [Collection(nameof(IntegrationApiTestFixtureCollection))]
    public class CountryApiTest : IntegrationTestBase
    {
        private readonly IntegrationTestFixture<StartupApiTests> _integrationTestFixture;

        private string _requestAdd => "api/Country/Add";
        private string _requestStateAdd => "api/State/Add";
        private string _requestGetById => "api/Country/GetById/";
        private string _requestStateGetById => "api/State/GetById/";
        private string _requestGetByName => "api/Country/GetByName?name=";
        private string _requestUpdate => "api/Country/Update/";
        private string _requestDelete => "api/Country/Delete/";
        private string _requestStateDelete => "api/State/Delete/";

        private readonly CountryTestFixture _countryTestFixture;

        public CountryApiTest(IntegrationTestFixture<StartupApiTests> integrationTestFixture) : base(integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
            _countryTestFixture = new CountryTestFixture();
        }

        #region Function Add

        [Fact(DisplayName = "Add new country with success"), Priority(2)]
        [Trait("UnitTests - Integration", "Country")]
        public async Task Country_AddNewCountry_ShouldHaveSuccess()
        {
            //Arrange, act and assert
            await AddNewCountriesAsync(1);
        }

        [Fact(DisplayName = "Add new Country with fail"), Priority(2)]
        [Trait("UnitTests - Integration", "Country")]
        public async Task Country_AddNewCountry_ShouldHadFailEndReturnNotifications()
        {
            //Arrange
            var countryViewModel = new CountryViewModel();

            //Act
            var postResponse = await _integrationTestFixture.Client.PostAsJsonAsync(_requestAdd, countryViewModel);
            var jsonResponse = await postResponse.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<Deserialize<CountryViewModel>>(jsonResponse);

            //Assert
            Assert.False(obj.Success);
            Assert.True(obj.Errors.Any());
            Assert.Contains(ConstantMessages.CountryNameRequired_PT, obj.Errors);
        }

        #endregion Function Add

        #region Function Get

        [Fact(DisplayName = "Get all countries with success"), Priority(3)]
        [Trait("UnitTests - Integration", "Country")]
        public async Task GetAllCountries_Add10NewCountriesAndGetAll_ShouldAddAndReturnAllActiveCountriesWithSuccess()
        {
            //Arrange and Act
            var countriesViewModel = await GetAllAsync<DeserializeList<CountryViewModel>>(GetRequestToGetAll(1, 10));
            var countryViewModel = countriesViewModel.Data.FirstOrDefault();

            _integrationTestFixture.CountryId = countryViewModel.Id.GetValueOrDefault();

            //Assert
            Assert.True(countriesViewModel.Data.Any());
        }

        [Theory(DisplayName = "Get all countries with pagination"), Priority(3)]
        [Trait("UnitTests - Integration", "Country")]
        [InlineData(50, 10)]
        [InlineData(100, 10)]
        [InlineData(1000, 5)]
        public async Task GetAllCountries_AddNewCountriesAndGetAllWithPagination_ShouldAddAllTheCountriesAndGetItWithPagination(int qtyCuntries, int qtyItemsPage)
        {
            //Arrage
            await AddNewCountriesAsync(qtyCuntries);
            int qtyPages = qtyCuntries / qtyItemsPage;
            var countriesRegistred = new List<CountryViewModel>();

            for (int i = 1; i <= qtyPages; i++)
            {
                //Act
                var countriesViewModel = await GetAllAsync<DeserializeList<CountryViewModel>>(GetRequestToGetAll(i, qtyItemsPage));

                //Assert
                Assert.Equal(qtyItemsPage, countriesViewModel.Data.Count);
                countriesViewModel.Data.ForEach(c => Assert.DoesNotContain(c, countriesRegistred));

                countriesRegistred.AddRange(countriesViewModel.Data);
            }
        }

        [Fact(DisplayName = "Get all countries with invalid page and page size"), Priority(3)]
        [Trait("UnitTests - Integration", "Country")]
        public async Task GetAllCountries_GetAllWithInvalidPageAndPageSize_ShouldNotReturnCountriesAndReturnNotifications()
        {
            //Arrange and act
            var response = await GetAllAsync<DeserializeList<CountryViewModel>>(GetRequestToGetAll(0, 0), false);

            //Assert
            Assert.True(response.Errors.Any());
            Assert.Contains(ConstantMessages.pageAndPageSizeRequired, response.Errors);
        }

        [Fact(DisplayName = "Get country by id with success"), Priority(4)]
        [Trait("UnitTests - Integration", "Country")]
        public async Task GetById_GetCountryById_ShouldReturnWithSuccess()
        {
            //Arrange and Act
            var countryViewModel = await GetCountryByIdAsync(_integrationTestFixture.CountryId);

            //Assert
            Assert.NotNull(countryViewModel);
            Assert.Equal(countryViewModel.Id, _integrationTestFixture.CountryId);
        }

        [Fact(DisplayName = "Get country by name with success"), Priority(4)]
        [Trait("UnitTests - Integration", "Country")]
        public async Task GetByName_GetCountriesByName_ShouldReturnWithSuccess()
        {
            //Arrange and act
            var countries = await GetAllAsync<DeserializeList<CountryViewModel>>(GetRequestToGetAll(1, 100));
            var country = TestTools.GetRandomEntityInList<CountryViewModel>(countries.Data);

            var response = await _integrationTestFixture.Client.GetAsync(_requestGetByName + country.Name);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var obj = DeserializeJson<DeserializeList<CountryViewModel>>(jsonResponse);

            //Assert
            Assert.True(obj.Data.Any());
            Assert.Contains(country.Name, obj.Data.Select(d => d.Name));
        }

        #endregion Function Get

        #region Function Update

        [Fact(DisplayName = "Update country with success"), Priority(5)]
        [Trait("UnitTests - Integration", "Country")]
        public async Task Update_UpdateValidCountry_ShouldUpdatedWithSuccess()
        {
            //Arrange
            var country = await GetCountryByIdAsync(_integrationTestFixture.CountryId);
            country.Name += " Nome Alterado";

            //Act
            await UpdateAsync<CountryViewModel>(country,
                                                country.Id.GetValueOrDefault(),
                                                _requestUpdate,
                                                true);

            //Assert
            var countryUpdated = await GetCountryByIdAsync(country.Id.GetValueOrDefault());
            CompareObjectCountry(country, countryUpdated);
        }

        [Fact(DisplayName = "Update country with fail"), Priority(5)]
        [Trait("UnitTests - Integration", "Country")]
        public async Task Update_UpdateInvalidCountry_ShouldUpdatedWithFailEndReturnNotifications()
        {
            //Arrange
            var countryViewModel = new CountryViewModel
            {
                Id = _integrationTestFixture.CountryId,
                Name = "a"
            };

            //Act
            var returnUpdate = await UpdateAsync<CountryViewModel>(countryViewModel,
                                                                   countryViewModel.Id.GetValueOrDefault(),
                                                                   _requestUpdate);

            //Assert
            Assert.False(returnUpdate.Success);
            Assert.True(returnUpdate.Errors.Any());
            Assert.Contains(ConstantMessages.CountryNameLengh_PT, returnUpdate.Errors);
        }

        #endregion Function Update

        #region Function Remove

        [Fact(DisplayName = "Remove country with success"), Priority(6)]
        [Trait("UnitTests - Integration", "Country")]
        public async Task Country_RemoveCountry_ShouldHadSuccess()
        {
            var id = Guid.NewGuid();
            await AddNewCountriesAsync(1, id);

            var countryViewModel = await GetCountryByIdAsync(id);
            //Arrange and Act
            var responseDelete = await _integrationTestFixture.Client.DeleteAsync($"{_requestDelete}{countryViewModel.Id}");

            //Assert
            responseDelete.EnsureSuccessStatusCode();

            var countryDeleted = await GetCountryByIdAsync(id);
            Assert.Null(countryDeleted);
        }

        [Fact(DisplayName = "Remove country with invalid id"), Priority(6)]
        [Trait("UnitTests - Integration", "Country")]
        public async Task RemoveCountry_RemoveCountryWithInvalidId_MustNotRemoveAndReturnMessageNotification()
        {
            //Arrange and Act
            var responseDelete = await _integrationTestFixture.Client.DeleteAsync($"{_requestDelete}{Guid.NewGuid()}");
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<CountryViewModel>>(responseDelete);

            //Assert
            Assert.False(responseDeserialized.Success);
            Assert.NotEmpty(responseDeserialized.Errors);
        }

        [Fact(DisplayName = "Remove invalid country with fail"), Priority(6)]
        [Trait("UnitTests - Integration", "Country")]
        public async Task Remove_RemoveInvalidCountry_ShouldNotRemoveAndNotifyErrors()
        {
            var responseDelete = await RemoveAsync(Guid.NewGuid(), _requestDelete);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, responseDelete.StatusCode);
        }

        [Fact(DisplayName = "Remove country associated with state"), Priority(6)]
        [Trait("UnitTests - Integration", "Country")]
        public async Task Remove_RemoveCountryWithStateAssociated_ShouldNotRemoveAndNotifyErrorValidation()
        {
            //arrange
            var stateId = Guid.NewGuid();
            var countryId = Guid.NewGuid();

            await AddNewCountriesAsync(1, countryId);
            await AddNewStateAsync(stateId, countryId);

            //Act
            var response = await RemoveAsync(countryId, _requestDelete);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "Remove country and state associated"), Priority(6)]
        [Trait("UnitTests - Integration", "Country")]
        public async Task Remove_RemoveCountryAndRemoveState_ShouldRemoveWithSucess()
        {
            //act
            var stateId = Guid.NewGuid();
            var countryId = Guid.NewGuid();

            await AddNewCountriesAsync(1, countryId);
            await AddNewStateAsync(stateId, countryId);

            //act
            var responseState = await RemoveAsync(stateId, _requestStateDelete);
            var responseCountry = await RemoveAsync(countryId, _requestDelete);

            //Assert
            responseCountry.EnsureSuccessStatusCode();
            responseState.EnsureSuccessStatusCode();
        }

        #endregion Function Remove

        #region Private methods

        internal async Task AddNewCountriesAsync(int qty, Guid? id = null)
        {
            //Arrange
            var countryFixture = new CountryTestFixture();
            var countriesViewModel = countryFixture.GenerateCountryViewModel(qty, id);
            await AddListAsync<CountryViewModel>(countriesViewModel, _requestAdd);
        }

        private async Task<CountryViewModel> GetCountryByIdAsync(Guid id)
        {
            var country = await GetByIdAsync<Deserialize<CountryViewModel>>(id, _requestGetById);
            return country?.Data;
        }

        private async Task<StateViewModel> GetStateByIdAsync(Guid id)
        {
            var state = await GetByIdAsync<Deserialize<StateViewModel>>(id, _requestStateGetById);
            return state?.Data;
        }

        private string GetRequestToGetAll(int page, int pageSize)
        {
            return $"api/Country/GetAll?page={page}&pageSize={pageSize}";
        }

        private static void CompareObjectCountry(CountryViewModel country,
                                                 CountryViewModel countryUpdated)
        {
            Assert.Equal(country.Id.GetValueOrDefault(), countryUpdated.Id.GetValueOrDefault());
            Assert.Equal(country.Name, countryUpdated.Name);
        }

        private async Task AddNewStateAsync(Guid idState, Guid countryId)
        {
            var stateFixture = new StateTestFixture();
            var stateViewModel = stateFixture.GenerateValidStateViewModel(idState, countryId);
            await AddAsync<StateViewModel>(stateViewModel, _requestStateAdd);
        }

        #endregion Private methods
    }
}