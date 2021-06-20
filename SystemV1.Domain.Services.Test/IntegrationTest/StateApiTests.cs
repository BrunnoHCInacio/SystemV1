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

namespace SystemV1.Domain.Test.IntegrationTest
{
    [TestCaseOrderer("SystemV1.Domain.Test.IntegrationTest.Config.PriorityOrderer", "SystemV1.Domain.Test.IntegrationTest")]
    [Collection(nameof(IntegrationApiTestFixtureCollection))]
    public class StateApiTests
    {
        private readonly IntegrationTestFixture<StartupApiTests> _integrationTestFixture;

        public StateApiTests(IntegrationTestFixture<StartupApiTests> integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
        }

        [Fact(DisplayName = "Add new state with success"), TestPriority(1)]
        [Trait("Categoria", "Integração - Estado")]
        public async Task State_AddNewState_ShoulHaveSuccess()
        {
            //Arrange
            _integrationTestFixture.IdForRetreave = new Guid("90750fed-e3b6-4fbc-a365-17ae694c91b9");
            var stateFixture = new StateTestFixture();
            var state = stateFixture.GenerateValidStateViewModel();
            var responseCountry = await _integrationTestFixture.Client.GetAsync($"api/country/GetById/{_integrationTestFixture.IdForRetreave}");
            responseCountry.EnsureSuccessStatusCode();
            
            var jsonCountry = await responseCountry.Content.ReadAsStringAsync();
            var countryViewModel = JsonConvert.DeserializeObject<CountryDeserialize>(jsonCountry)?.Data;

            state.CountryId = countryViewModel.Id.GetValueOrDefault();

            //Act
            var postResponse = await _integrationTestFixture.Client.PostAsJsonAsync("api/state/Add", state);

            //Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Get all states with success"), TestPriority(2)]
        [Trait("Categoria", "Integração - Estado")]
        public async Task State_GetAllStates_ShoulReturnWithSuccess()
        {
            //Arrange & Act
            var response = await _integrationTestFixture.Client.GetAsync("api/state/GetAll?page=1&pageSize=10");
            var jsonStates = await response.Content.ReadAsStringAsync();

            //Assert
            response.EnsureSuccessStatusCode();

            var states = JsonConvert.DeserializeObject<StatesDeserialize>(jsonStates).States;
            var index = new Random().Next(1, states.Count);
            var stateViewModel = states.ElementAt(index);
            _integrationTestFixture.IdForRetreave = stateViewModel.Id;

        }

        [Fact(DisplayName = "get state by id with success"), TestPriority(3)]
        [Trait("Categoria", "Integração - Estado")]
        public async Task State_GetStateById_ShoulHaveReturnWithSuccess()
        {
            //Arrange & Act
            var response = await _integrationTestFixture.Client.GetAsync($"api/state/GetById{_integrationTestFixture.IdForRetreave}");
            var jsonState = await response.Content.ReadAsStringAsync();

            //Assert
            response.EnsureSuccessStatusCode();
            var state = JsonConvert.DeserializeObject<StateDeserialize>(jsonState).State;
            Assert.NotNull(state);
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

        [JsonProperty("data")]
        public StateViewModel State { get; set; }
    }
}