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
    public class StateApiTests : IntegrationTestBase
    {
        private readonly IntegrationTestFixture<StartupApiTests> _integrationTestFixture;

        private string _requestAdd => "api/state/Add";
        private string _requestCityAdd => "api/city/Add";
        private string _requestGetById => "api/state/GetById/";
        private string _requestGetStateCountryById => "api/state/GetStateCountryById/";
        private string requestGetCountryById => "api/country/GetById/";
        private string requestUpdate => "api/state/Update/";
        private string _requestDelete => "api/state/Delete/";
        private string _requestCityDelete => "api/city/Delete/";

        public StateApiTests(IntegrationTestFixture<StartupApiTests> integrationTestFixture) : base(integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
        }

        #region Function Add

        [Fact(DisplayName = "Add new state with success"), Priority(1)]
        [Trait("UnitTests - Integration", "State")]
        public async Task State_AddNewState_ShoulHaveSuccess()
        {
            //Arrange, Act and Assert
            await AddNewStateAsync(1);
        }

        [Fact(DisplayName = "Add new invalid state"), Priority(1)]
        [Trait("UnitTests - Integration", "State")]
        public async Task Add_AddNewInvalidState_ShouldNotAddAndReturnValidationMessages()
        {
            //Arrange
            var stateViewModel = new StateViewModel();

            //Act
            var responseDeserialized = await AddStateAsync(stateViewModel, false);

            //Assert
            Assert.False(responseDeserialized.Success);
            Assert.Contains(ConstantMessages.StateNameRequired_Pt, responseDeserialized.Errors);
        }

        #endregion Function Add

        #region Function Get

        [Fact(DisplayName = "Get all states with success"), Priority(2)]
        [Trait("UnitTests - Integration", "State")]
        public async Task State_GetAllStates_ShoulReturnWithSuccess()
        {
            //Arrange, Act and Assert
            var statesViewModel = await GetAllStatesAsync(1, 5);
            var stateViewModel = statesViewModel.FirstOrDefault();
            _integrationTestFixture.StateId = stateViewModel.Id;
        }

        [Theory(DisplayName = "Get all states with pagination")]
        [Trait("UnitTests - Integration", "State")]
        [InlineData(50, 5)]
        [InlineData(10, 2)]
        public async Task GetAll_GetAllStatesWithPagination(int qtyStates, int qtyItemsPage)
        {
            await AddNewStateAsync(qtyStates);

            int qtyPages = qtyStates / qtyItemsPage;
            var statesRegistred = new List<StateViewModel>();

            for (int i = 1; i <= qtyPages; i++)
            {
                //Act
                var statesViewModel = await GetAllStatesAsync(i, qtyItemsPage);

                //Assert
                Assert.Equal(qtyItemsPage, statesViewModel.Count);
                statesViewModel.ForEach(c => Assert.DoesNotContain(c, statesRegistred));

                statesRegistred.AddRange(statesViewModel);
            }
        }

        [Fact(DisplayName = "Get state by id with success"), Priority(3)]
        [Trait("UnitTests - Integration", "State")]
        public async Task State_GetStateById_ShoulHaveReturnWithSuccess()
        {
            //Arrange & Act
            var response = await GetByIdAsync<Deserialize<StateViewModel>>(_integrationTestFixture.StateId, _requestGetById);

            //Assert
            Assert.NotNull(response.Data);
        }

        [Fact(DisplayName = "Get state by id with invalid id")]
        [Trait("UnitTests - Integration", "State")]
        public async Task GetById_GetStateByIdWithInvalidId_ShouldReturnMessageInformation()
        {
            //Arrange and act
            var stateDeserialized = await GetStateByIdAsync(Guid.Empty);

            //Assert
            Assert.Null(stateDeserialized);
        }

        [Fact(DisplayName = "Get state with country by id with success"), Priority(3)]
        [Trait("UnitTests - Integration", "State")]
        public async Task GetStateCountryById_GetStateWithCountryById_ShoulReturnWithSuccess()
        {
            //Arrange & Act
            var stateDeserialized = await GetStateByIdAsync(_integrationTestFixture.StateId, true);

            //Assert
            Assert.NotNull(stateDeserialized.Data);
            Assert.NotEqual(Guid.Empty, stateDeserialized.Data.CountryId);
            Assert.NotEmpty(stateDeserialized.Data.CountryName);
        }

        [Fact(DisplayName = "Get state country by id with success"), Priority(3)]
        [Trait("UnitTests - Integration", "State")]
        public async Task State_GetStateCountryById_ShoulHaveReturnWithSuccess()
        {
            //Arrange & Act
            var stateDeserialized = await GetStateByIdAsync(_integrationTestFixture.StateId, true);

            //Assert
            Assert.NotNull(stateDeserialized.Data);
        }

        #endregion Function Get

        #region Function update

        [Fact(DisplayName = "Update state with success"), Priority(4)]
        [Trait("UnitTests - Integration", "State")]
        public async Task State_UpdateState_ShouldHadSuccess()
        {
            //Arrange
            var stateDeserialized = await GetStateByIdAsync(_integrationTestFixture.StateId, true);
            var stateViewModel = stateDeserialized.Data;
            stateViewModel.Name = "Estado alterado";

            //Act
            await UpdateStateAsync(stateViewModel);

            //Assert
            var responseUpdated = await GetByIdAsync<Deserialize<StateViewModel>>(stateViewModel.Id, _requestGetById);

            var stateViewModelUpdated = responseUpdated.Data;

            Assert.Equal(stateViewModel.Id, stateViewModelUpdated.Id);
            Assert.Equal(stateViewModel.Name, stateViewModelUpdated.Name);
        }

        [Fact(DisplayName = "Update country in valid state"), Priority(4)]
        [Trait("UnitTests - Integration", "State")]
        public async Task Update_UpdateCountryInValidState_ShouldUpdateWithSuccess()
        {
            //Arrange
            var stateDeserialized = await GetStateByIdAsync(_integrationTestFixture.StateId, true);
            var stateViewModel = stateDeserialized.Data;

            var countries = await GetAllCountriesAsync(1, 15);
            var country = TestTools.GetRandomEntityInList(countries);

            stateViewModel.CountryId = country.Id.GetValueOrDefault();

            //Act
            await UpdateStateAsync(stateViewModel, false);

            //Assert
            var stateUpdatedDeserialized = await GetStateByIdAsync(_integrationTestFixture.StateId, true);
            var stateUpdatedViewModel = stateUpdatedDeserialized.Data;

            Assert.Equal(stateUpdatedViewModel.CountryId, country.Id.GetValueOrDefault());
            Assert.Equal(stateUpdatedViewModel.CountryName, country.Name);
        }

        [Fact(DisplayName = "Update invalid state"), Priority(4)]
        [Trait("UnitTests - Integration", "State")]
        public async Task Update_UpdateStateWithInvalidArguments_ShouldNotUpdateAndReturnNotifications()
        {
            //Arrange
            var stateDeserialized = await GetStateByIdAsync(_integrationTestFixture.StateId);
            var stateViewModel = stateDeserialized.Data;

            stateViewModel.Name = "a";
            stateViewModel.CountryId = Guid.Empty;

            //Act
            var stateDeserialize = await UpdateStateAsync(stateViewModel, false);

            //Assert
            Assert.False(stateDeserialize.Success);
            Assert.True(stateDeserialize.Errors.Any());
            Assert.Contains(ConstantMessages.StateNameLenght_Pt, stateDeserialize.Errors);
        }

        #endregion Function update

        #region Function Delete

        [Fact(DisplayName = "Delete state with success"), Priority(5)]
        [Trait("UnitTests - Integration", "State")]
        public async Task State_DeleteState_ShouldHadSuccess()
        {
            //Arrange
            var stateId = Guid.NewGuid();
            await AddNewStateAsync(1, stateId);

            //Act
            var responseDelete = await RemoveAsync(stateId,
                                                   _requestDelete);

            //Assert
            responseDelete.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Delete state with invalid id"), Priority(5)]
        [Trait("UnitTests - Integration", "State")]
        public async Task Delete_DeleteStateWithInvalidId_ShouldNotDeleteAndReturnNotifications()
        {
            //Act
            var responseDelete = await _integrationTestFixture.Client.DeleteAsync($"{_requestDelete}{Guid.NewGuid()}");
            var jsonResponse = await responseDelete.Content.ReadAsStringAsync();
            var stateDeserialized = JsonConvert.DeserializeObject<Deserialize<StateViewModel>>(jsonResponse);

            //Assert
            Assert.False(stateDeserialized.Success);
        }

        [Fact(DisplayName = "Delete state associated with city"), Priority(5)]
        [Trait("UnitTests - Integration", "State")]
        public async Task Remove_RemoveStateAssociatedWithCity_ShouldNotRemove()
        {
            //Arrange
            var stateId = Guid.NewGuid();
            var cityId = Guid.NewGuid();

            await AddNewStateAsync(1, stateId);

            var cityFixture = new CityTestFixture();
            var cityViewModel = cityFixture.GenerateValidCityViewModel(1, stateId, cityId);

            await AddAsync<CityViewModel>(cityViewModel.FirstOrDefault(), _requestCityAdd);

            //Act
            var response = await RemoveAsync(stateId, _requestDelete);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact(DisplayName = "Delete state and city"), Priority(5)]
        [Trait("UnitTests - Integration", "State")]
        public async Task Remove_RemoveStateAndCity_ShouldRemoveWithSucess()
        {
            //Arrange
            var stateId = Guid.NewGuid();
            var cityId = Guid.NewGuid();

            await AddNewStateAsync(1, stateId);

            var cityFixture = new CityTestFixture();
            var cityViewModel = cityFixture.GenerateValidCityViewModel(1, stateId, cityId);

            await AddAsync<CityViewModel>(cityViewModel.FirstOrDefault(), _requestCityAdd);

            //Act
            var responseCity = await RemoveAsync(cityId, _requestCityDelete);
            var response = await RemoveAsync(stateId, _requestDelete);

            //Assert
            responseCity.EnsureSuccessStatusCode();
            response.EnsureSuccessStatusCode();
        }

        #endregion Function Delete

        public async Task<List<StateViewModel>> GetAllStatesAsync(int page, int pageSize)
        {
            var response = await GetAllAsync<DeserializeList<StateViewModel>>(PrepareRequestGetAllStates(page, pageSize));

            return response.Data;
        }

        #region Private Methods

        private async Task<Deserialize<StateViewModel>> UpdateStateAsync(StateViewModel stateViewModel, bool successCase = true)
        {
            var response = await _integrationTestFixture
                                                .Client
                                                .PutAsJsonAsync($"{requestUpdate}{stateViewModel.Id}", stateViewModel);

            if (successCase)
            {
                response.EnsureSuccessStatusCode();
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Deserialize<StateViewModel>>(jsonResponse);
        }

        private async Task<Deserialize<StateViewModel>> AddStateAsync(StateViewModel stateViewModel, bool successCase = true)
        {
            var response = await AddAsync<StateViewModel>(stateViewModel, _requestAdd, successCase);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Deserialize<StateViewModel>>(jsonResponse);
        }

        private async Task<List<CountryViewModel>> GetAllCountriesAsync(int page, int pageSize)
        {
            var response = await _integrationTestFixture.Client.GetAsync(PrepareRequestGetAllCountries(page, pageSize));
            var jsonContries = await response.Content.ReadAsStringAsync();
            var countriesViewModel = JsonConvert.DeserializeObject<DeserializeList<CountryViewModel>>(jsonContries);
            return countriesViewModel.Data;
        }

        internal async Task AddNewStateAsync(int qty, Guid? id = null)
        {
            for (int i = 0; i < qty; i++)
            {
                var countryApi = new CountryApiTest(_integrationTestFixture);
                await countryApi.AddNewCountriesAsync(1);

                var countriesViewModel = await GetAllCountriesAsync(1, 5);
                var countryViewModel = countriesViewModel.FirstOrDefault();

                var stateFixture = new StateTestFixture();
                var state = stateFixture.GenerateValidStateViewModel(id, countryViewModel.Id);

                await AddAsync<StateViewModel>(state, _requestAdd);
            }
        }

        private async Task<Deserialize<StateViewModel>> GetStateByIdAsync(Guid id, bool withState = false)
        {
            if (withState)
            {
                return await GetByIdAsync<Deserialize<StateViewModel>>(id, _requestGetStateCountryById);
            }
            return await GetByIdAsync<Deserialize<StateViewModel>>(id, _requestGetById);
        }

        private string PrepareRequestGetAllCountries(int page, int pageSize)
        {
            return $"api/country/GetAll?page={page}&pageSize={pageSize}";
        }

        private string PrepareRequestGetAllStates(int page, int pageSize)
        {
            return $"api/state/GetAll?page={page}&pageSize={pageSize}";
        }

        #endregion Private Methods
    }
}