using API2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Test.Fixture;
using SystemV1.Domain.Test.IntegrationTest.Config;
using Xunit;

namespace SystemV1.Domain.Test.IntegrationTest
{
    [TestCaseOrderer("SystemV1.Domain.Test.IntegrationTest.Config.PriorityOrderer", "SystemV1.Domain.Test.IntegrationTest")]
    [Collection(nameof(IntegrationApiTestFixtureCollection))]
    // [CollectionDefinition(DisableParallelization = true)]
    public class CountryApiTest
    {
        private readonly IntegrationTestFixture<StartupApiTests> _integrationTestFixture;

        public CountryApiTest(IntegrationTestFixture<StartupApiTests> integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
        }

        [Fact(DisplayName = "Add new country with success"), TestPriority(1)]
        [Trait("Categoria", "Integração - Pais")]
        public async Task Country_AddNewCountry_ShouldHaveSuccess()
        {
            //Arrange
            var countryFixture = new CountryTestFixture();
            var countryViewModel = countryFixture.GenerateValidContryViewModel();

            //Act
            var postResponse = await _integrationTestFixture.Client.PostAsJsonAsync("api/country/Add", countryViewModel);

            //Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Get all countries with success"), TestPriority(2)]
        [Trait("Categoria", "Integração - Pais")]
        public async Task Country_GetAllCountries_ShouldReturnWithSuccess()
        {
            //Arrange & act
            var response = await _integrationTestFixture.Client.GetAsync("api/Country/GetAll?page=1&pageSize=10");
            string jsonCountries = await response.Content.ReadAsStringAsync();

            var countriesViewModel = JsonConvert.DeserializeObject<CountriesDeserialize>(jsonCountries);
            int index = new Random().Next(1, countriesViewModel.Data.Count);

            var countryViewModel = countriesViewModel.Data.ElementAt(index);
            _integrationTestFixture.IdForRetreave = countryViewModel.Id.GetValueOrDefault();
            //Assert
            Assert.NotEmpty(jsonCountries);
        }

        [Fact(DisplayName = "Get country by id with success"), TestPriority(3)]
        [Trait("Categoria", "Integração - Pais")]
        public async Task Country_GetCountryById_ShouldReturnWithSuccess()
        {
            //Arrange & act
            var response = await _integrationTestFixture.Client.GetAsync($"api/Country/GetById/{_integrationTestFixture.IdForRetreave}");
            var jsonCountry = await response.Content.ReadAsStringAsync();

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Update country with success"), TestPriority(4)]
        [Trait("Categoria", "Integração - Pais")]
        public async Task Country_UpdateCountry_ShouldUpdatedWithSuccess()
        {
            //Arrange
            var response = await _integrationTestFixture
                                    .Client
                                    .GetAsync($"api/country/GetById/{_integrationTestFixture.IdForRetreave}");

            var jsonCountry = await response.Content.ReadAsStringAsync();
            var countryViewModel = JsonConvert.DeserializeObject<CountryDeserialize>(jsonCountry)?.Data;
            response.EnsureSuccessStatusCode();

            countryViewModel.Name = "Nome Alterado";

            //Act
            var responseUpdate = await _integrationTestFixture
                                            .Client
                                            .PutAsJsonAsync($"api/country/Update/{countryViewModel.Id}", countryViewModel);

            //Assert
            responseUpdate.EnsureSuccessStatusCode();
            var responseUpdated = await _integrationTestFixture
                                                .Client
                                                .GetAsync($"api/country/GetByid/{countryViewModel.Id}");
            var jsonCountryUpdated = await responseUpdated.Content.ReadAsStringAsync();
            var countryUpdatedViewModel = JsonConvert.DeserializeObject<CountryDeserialize>(jsonCountryUpdated)?.Data;

            Assert.Equal(countryViewModel.Id.GetValueOrDefault(), countryUpdatedViewModel.Id.GetValueOrDefault());
            Assert.Equal(countryViewModel.Name, countryUpdatedViewModel.Name);
        }
    }

    public class CountriesDeserialize
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public List<CountryViewModel> Data = new List<CountryViewModel>();
    }

    public class CountryDeserialize
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public CountryViewModel Data { get; set; }
    }
}