using API2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Application.Resources;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Services.Test.Fixture;
using SystemV1.Domain.Test.IntegrationTest.Config;
using Xunit;
using Xunit.Priority;

namespace SystemV1.Domain.Test.IntegrationTest
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    [Collection(nameof(IntegrationApiTestFixtureCollection))]
    public class StateApiTests
    {
        private readonly IntegrationTestFixture<StartupApiTests> _integrationTestFixture;
        
        private string requestAdd => "api/state/Add";
        private string requestGetById => "api/state/GetById/";
        private string requestGetStateCountryById => "api/state/GetStateCountryById/";
        private string requestGetCountryById => "api/country/GetById/";
        private string requestUpdate => "api/state/Update/";
        private string requestDelete => "api/state/Delete/";

        public StateApiTests(IntegrationTestFixture<StartupApiTests> integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
        }

        #region Function Add

        [Fact(DisplayName = "Add new state with success"), Priority(1)]
        [Trait("Categoria", "Estado - Integração")]
        public async Task State_AddNewState_ShoulHaveSuccess()
        {
            //Arrange, Act and Assert
            await AddNewStateAsync(1);
        }

        [Fact(DisplayName ="Add new invalid state"), Priority(1)]
        [Trait("Categoria", "Estado - Integração")]
        public async Task Add_AddNewInvalidState_ShouldNotAddAndReturnValidationMessages()
        {
            //Arrange
            var stateViewModel = new StateViewModel();

            //Act
            StateDeserialize responseDeserialized = await AddStateAsync(stateViewModel, false);

            //Assert
            Assert.False(responseDeserialized.Success);
            Assert.Contains(ConstantMessages.StateNameRequired_Pt, responseDeserialized.Errors);
        }

        #endregion

        #region Function Get

        [Fact(DisplayName = "Get all states with success"), Priority(2)]
        [Trait("Categoria", "Estado - Integração")]
        public async Task State_GetAllStates_ShoulReturnWithSuccess()
        {
            //Arrange, Act and Assert
            var statesViewModel = await GetAllStatesAsync(1,5);
            var stateViewModel = statesViewModel.FirstOrDefault();
            _integrationTestFixture.StateId = stateViewModel.Id;
        }

        [Theory(DisplayName ="Get all states with pagination")]
        [Trait("Categoria", "Estado - Integração")]
        [InlineData(50,5)]
        [InlineData(10,2)]
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
        [Trait("Categoria", "Estado - Integração")]
        public async Task State_GetStateById_ShoulHaveReturnWithSuccess()
        {
            //Arrange & Act
            var response = await _integrationTestFixture.Client.GetAsync(PrepareRequestGetById(_integrationTestFixture.StateId));
            var jsonState = await response.Content.ReadAsStringAsync();

            //Assert
            response.EnsureSuccessStatusCode();
            var stateDeserialized = JsonConvert.DeserializeObject<StateDeserialize>(jsonState);
            Assert.NotNull(stateDeserialized.Data);
        }

        private string PrepareRequestGetById(Guid id)
        {
            return $"{requestGetById}{id}";
        }

        [Fact(DisplayName = "Get state by id with invalid id")]
        [Trait("Categoria", "Estado - Integração")]
        public async Task GetById_GetStateByIdWithInvalidId_ShouldReturnMessageInformation()
        {
            //Arrange and act
            var stateDeserialized = await GetStateByIdAsync(Guid.Empty);

            //Assert
            Assert.Null(stateDeserialized);
        }

        [Fact(DisplayName = "Get state with country by id with success"), Priority(3)]
        [Trait("Categoria", "Estado - Integração")]
        public async Task GetStateCountryById_GetStateWithCountryById_ShoulReturnWithSuccess()
        {
            //Arrange & Act
            var stateDeserialized = await GetStateByIdAsync(_integrationTestFixture.StateId);

            //Assert
            Assert.NotNull(stateDeserialized.Data);
            Assert.NotEqual(Guid.Empty, stateDeserialized.Data.CountryId);
            Assert.NotEmpty(stateDeserialized.Data.CountryName);
        }

        [Fact(DisplayName = "Get state country by id with success"), Priority(3)]
        [Trait("Categoria", "Estado - Integração")]
        public async Task State_GetStateCountryById_ShoulHaveReturnWithSuccess()
        {
            //Arrange & Act
            var response = await _integrationTestFixture.Client.GetAsync($"{requestGetStateCountryById}{_integrationTestFixture.StateId}");
            var jsonState = await response.Content.ReadAsStringAsync();

            //Assert
            response.EnsureSuccessStatusCode();
            var stateDeserialized = JsonConvert.DeserializeObject<StateDeserialize>(jsonState);
            Assert.NotNull(stateDeserialized.Data);
        }

        #endregion

        #region Function update

        [Fact(DisplayName = "Update state with success"), Priority(4)]
        [Trait("Categoria", "Estado - Integração")]
        public async Task State_UpdateState_ShouldHadSuccess()
        {
            //Arrange
            var stateDeserialized = await GetStateByIdAsync(_integrationTestFixture.StateId);
            var stateViewModel = stateDeserialized.Data;
            stateViewModel.Name = "Estado alterado";

            //Act
            var responseUpdateState = await UpdateStateAsync(stateViewModel);

            //Assert
            var responseGetStateUpdated = await _integrationTestFixture.Client.GetAsync($"{requestGetById}{stateViewModel.Id}");
            var jsonResponseGetStateUpdated = await responseGetStateUpdated.Content.ReadAsStringAsync();
            var stateViewModelUpdated = JsonConvert.DeserializeObject<StateDeserialize>(jsonResponseGetStateUpdated)?.Data;

            Assert.Equal(stateViewModel.Id, stateViewModelUpdated.Id);
            Assert.Equal(stateViewModel.Name, stateViewModelUpdated.Name);
        }

        [Fact(DisplayName = "Update country in valid state"), Priority(4)]
        [Trait("Categoria", "Estado - Integração")]
        public async Task Update_UpdateCountryInValidState_ShouldUpdateWithSuccess()
        {
            //Arrange
            var stateDeserialized = await GetStateByIdAsync(_integrationTestFixture.StateId);
            var stateViewModel = stateDeserialized.Data;

            var countries = await GetAllCountriesAsync(1, 15);
            var country = TestTools.GetRandomEntityInList(countries);

            stateViewModel.CountryId = country.Id.GetValueOrDefault();

            //Act
            var stateDeserialize = await UpdateStateAsync(stateViewModel, false);

            //Assert
            var stateUpdatedDeserialized = await GetStateByIdAsync(_integrationTestFixture.StateId);
            var stateUpdatedViewModel = stateUpdatedDeserialized.Data;

            Assert.Equal(stateUpdatedViewModel.CountryId, country.Id);
            Assert.Equal(stateUpdatedViewModel.CountryName, country.Name);
        }

        [Fact(DisplayName ="Update invalid state"), Priority(4)]
        [Trait("Categoria","Estado - Integração")]
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

        #endregion

        #region Function Delete

        [Fact(DisplayName = "Delete state with success"), Priority(5)]
        [Trait("Categoria", "Estado - Integração")]
        public async Task State_DeleteState_ShouldHadSuccess()
        {
            //Arrange and Act
            var responseDelete = await _integrationTestFixture.Client.DeleteAsync($"{requestDelete}{_integrationTestFixture.StateId}");

            //Assert
            responseDelete.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Delete state with invalid id"), Priority(5)]
        [Trait("Categoria", "Estado - Integração")]
        public async Task Delete_DeleteStateWithInvalidId_ShouldNotDeleteAndReturnNotifications()
        {
            //Act
            var responseDelete = await _integrationTestFixture.Client.DeleteAsync($"{requestDelete}{Guid.NewGuid()}");
            var jsonResponse = await responseDelete.Content.ReadAsStringAsync();
            var stateDeserialized = JsonConvert.DeserializeObject<StateDeserialize>(jsonResponse);

            //Assert
            Assert.False(stateDeserialized.Success);
        }

        #endregion

        #region Private Methods

        private async Task<StateDeserialize> UpdateStateAsync(StateViewModel stateViewModel, bool successCase = true)
        {
            var response = await _integrationTestFixture
                                                .Client
                                                .PutAsJsonAsync($"{requestUpdate}{stateViewModel.Id}", stateViewModel);
            
            if (successCase)
            {
                response.EnsureSuccessStatusCode();
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<StateDeserialize>(jsonResponse);
        }

        private async Task<StateDeserialize> AddStateAsync(StateViewModel stateViewModel, bool successCase = true)
        {
            var response = await _integrationTestFixture.Client.PostAsJsonAsync(requestAdd, stateViewModel);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            if (successCase)
            {
                response.EnsureSuccessStatusCode();
            }
            return JsonConvert.DeserializeObject<StateDeserialize>(jsonResponse);
        }

        private async Task<List<CountryViewModel>> GetAllCountriesAsync(int page,int pageSize)
        {
            var response = await _integrationTestFixture.Client.GetAsync(PrepareRequestGetAllCountries(page, pageSize));
            var jsonContries = await response.Content.ReadAsStringAsync();
            var countriesViewModel = JsonConvert.DeserializeObject<CountriesDeserialize>(jsonContries);
            return countriesViewModel.Data;
        }

        internal async Task AddNewStateAsync(int qty)
        {
            for (int i = 0; i < qty; i++)
            {
                var countryApi = new CountryApiTest(_integrationTestFixture);
                await countryApi.AddNewCountriesAsync(1, false);

                var countriesViewModel = await GetAllCountriesAsync(1, 5);
                var countryViewModel = countriesViewModel.FirstOrDefault();

                var stateFixture = new StateTestFixture();
                var state = stateFixture.GenerateValidStateViewModel();
                state.CountryId = countryViewModel.Id.GetValueOrDefault();

                var postResponse = await _integrationTestFixture.Client.PostAsJsonAsync(requestAdd, state);
                postResponse.EnsureSuccessStatusCode();
            }  
        }

        private async Task<List<StateViewModel>> GetAllStatesAsync(int page, int pageSize)
        {
            var response = await _integrationTestFixture.Client.GetAsync(PrepareRequestGetAllStates(page, pageSize));
            var jsonStates = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            var statesDeserialized = JsonConvert.DeserializeObject<StatesDeserialize>(jsonStates);
            return statesDeserialized.States;

        }

        private async Task<StateDeserialize> GetStateByIdAsync(Guid id)
        {
            var response = await _integrationTestFixture.Client.GetAsync(PrepareRequestGetById(id));
            var jsonState = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            var stateDeserialized = JsonConvert.DeserializeObject<StateDeserialize>(jsonState);
            return stateDeserialized;
        }

        private string PrepareRequestGetAllCountries(int page, int pageSize)
        {
            return $"api/country/GetAll?page={page}&pageSize={pageSize}";
        }

        private string PrepareRequestGetAllStates(int page, int pageSize)
        {
            return $"api/state/GetAll?page={page}&pageSize={pageSize}";
        }

        #endregion
    }

    public class StatesDeserialize
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public List<StateViewModel> States = new List<StateViewModel>();
    }

}