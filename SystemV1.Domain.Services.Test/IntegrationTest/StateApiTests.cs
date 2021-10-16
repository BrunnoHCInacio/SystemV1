using API2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        private string requestGetAll => "api/state/GetAll?page=1&pageSize=10";
        private string requestAdd => "api/state/Add";
        private string requestGetById => "api/state/GetById/";
        private string requestGetCountryById => "api/country/GetById/";
        private string requestGetAllCountries => "api/country/GetAll?page=1&pageSize=1000";
        private string requestGetStateCountryById => "api/state/GetStateCountryById/";
        private string requestUpdate => "api/state/Update/";
        private string requestDelete => "api/state/Delete/";

        public StateApiTests(IntegrationTestFixture<StartupApiTests> integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
        }

        [Fact(DisplayName = "Add new state with success"), Priority(1)]
        [Trait("Categoria", "Integração - Estado")]
        public async Task State_AddNewState_ShoulHaveSuccess()
        {
            //Arrange
            var response = await _integrationTestFixture.Client.GetAsync(requestGetAllCountries);
            var jsonContries = await response.Content.ReadAsStringAsync();
            var countriesViewModel = JsonConvert.DeserializeObject<CountriesDeserialize>(jsonContries)?.Data;
            var countryViewModel = TestTools.GetEntityByRandom<CountryViewModel>(countriesViewModel);

            var stateFixture = new StateTestFixture();
            var state = stateFixture.GenerateValidStateViewModel();

            state.CountryId = countryViewModel.Id.GetValueOrDefault();

            //Act
            var postResponse = await _integrationTestFixture.Client.PostAsJsonAsync(requestAdd, state);

            //Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Get all states with success"), Priority(2)]
        [Trait("Categoria", "Integração - Estado")]
        public async Task State_GetAllStates_ShoulReturnWithSuccess()
        {
            //Arrange & Act
            var response = await _integrationTestFixture.Client.GetAsync(requestGetAll);
            var jsonStates = await response.Content.ReadAsStringAsync();

            //Assert
            response.EnsureSuccessStatusCode();
            var statesViewModel = JsonConvert.DeserializeObject<StatesDeserialize>(jsonStates).States;
            var stateViewModel = TestTools.GetEntityByRandom<StateViewModel>(statesViewModel);
            _integrationTestFixture.StateId = stateViewModel.Id;
        }

        [Fact(DisplayName = "Get state by id with success"), Priority(3)]
        [Trait("Categoria", "Integração - Estado")]
        public async Task State_GetStateById_ShoulHaveReturnWithSuccess()
        {
            //Arrange & Act
            var response = await _integrationTestFixture.Client.GetAsync($"{requestGetById}{_integrationTestFixture.StateId}");
            var jsonState = await response.Content.ReadAsStringAsync();

            //Assert
            response.EnsureSuccessStatusCode();
            var state = JsonConvert.DeserializeObject<StateDeserialize>(jsonState).State;
            Assert.NotNull(state);
        }

        [Fact(DisplayName = "Get state country by id with success"), Priority(4)]
        [Trait("Categoria", "Integração - Estado")]
        public async Task State_GetStateCountryById_ShoulHaveReturnWithSuccess()
        {
            //Arrange & Act
            var response = await _integrationTestFixture.Client.GetAsync($"{requestGetStateCountryById}{_integrationTestFixture.StateId}");
            var jsonState = await response.Content.ReadAsStringAsync();

            //Assert
            response.EnsureSuccessStatusCode();
            var state = JsonConvert.DeserializeObject<StateDeserialize>(jsonState).State;
            Assert.NotNull(state);
        }

        [Fact(DisplayName = "Update state with success"), Priority(5)]
        [Trait("Categoria", "Integração - Estado")]
        public async Task State_UpdateState_ShouldHadSuccess()
        {
            //Arrange
            var responseState = await _integrationTestFixture.Client.GetAsync($"{requestGetStateCountryById}{_integrationTestFixture.StateId}");
            var jsonResponseState = await responseState.Content.ReadAsStringAsync();
            var stateViewModel = JsonConvert.DeserializeObject<StateDeserialize>(jsonResponseState).State;
            stateViewModel.Name = "Estado alterado";

            //Act
            var responseUpdateState = await _integrationTestFixture
                                                .Client
                                                .PutAsJsonAsync($"{requestUpdate}{stateViewModel.Id}", stateViewModel);

            //Assert
            responseUpdateState.EnsureSuccessStatusCode();

            var responseGetStateUpdated = await _integrationTestFixture.Client.GetAsync($"{requestGetById}{stateViewModel.Id}");
            var jsonResponseGetStateUpdated = await responseGetStateUpdated.Content.ReadAsStringAsync();
            var stateViewModelUpdated = JsonConvert.DeserializeObject<StateDeserialize>(jsonResponseGetStateUpdated)?.State;

            Assert.Equal(stateViewModel.Id, stateViewModelUpdated.Id);
            Assert.Equal(stateViewModel.Name, stateViewModelUpdated.Name);
        }

        [Fact(DisplayName = "Delete state with success"), Priority(6)]
        [Trait("Categoria", "Integração - Estado")]
        public async Task State_DeleteState_ShouldHadSuccess()
        {
            //Arrange
            var responseState = await _integrationTestFixture.Client.GetAsync($"{requestGetById}{_integrationTestFixture.StateId}");
            var jsonResponseState = await responseState.Content.ReadAsStringAsync();
            var stateViewModel = JsonConvert.DeserializeObject<StateDeserialize>(jsonResponseState).State;

            //Act
            var responseDelete = await _integrationTestFixture.Client.DeleteAsync($"{requestDelete}{stateViewModel.Id}");

            //Assert
            responseDelete.EnsureSuccessStatusCode();
        }
    }

    public class StatesDeserialize
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public List<StateViewModel> States = new List<StateViewModel>();
    }

    public class StateDeserialize
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("errors")]
        public List<string> Errors { get; set; }

        [JsonProperty("data")]
        public StateViewModel State { get; set; }
    }
}