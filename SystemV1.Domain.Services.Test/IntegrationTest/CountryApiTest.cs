using API2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SystemV1.Application.Resources;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Test.Fixture;
using SystemV1.Domain.Test.IntegrationTest.Config;
using SystemV1.Domain.Validations;
using Xunit;
using Xunit.Priority;

namespace SystemV1.Domain.Test.IntegrationTest
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    [Collection(nameof(IntegrationApiTestFixtureCollection))]
    public class CountryApiTest
    {
        private readonly IntegrationTestFixture<StartupApiTests> _integrationTestFixture;
        private string _requestAdd => "api/country/Add";
        private string _requestGetAll => "api/Country/GetAll?page=1&pageSize=10";
        private string _requestGetById => "api/Country/GetById/";
        private string _requestUpdate => "api/country/Update/";
        private string _requestDelete => "api/country/Delete/";

        public CountryApiTest(IntegrationTestFixture<StartupApiTests> integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
        }

        [Fact(DisplayName = "Add new country with success"), Priority(1)]
        [Trait("Categoria", "País - Integração")]
        public async Task Country_AddNewCountry_ShouldHaveSuccess()
        {
            //Arrange
            var countryFixture = new CountryTestFixture();
            var countryViewModel = countryFixture.GenerateValidContryViewModel();

            //Act
            var postResponse = await _integrationTestFixture.Client.PostAsJsonAsync(_requestAdd, countryViewModel);

            //Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Add new Country with fail"), Priority(2)]
        [Trait("Categoria", "País - Integração")]
        public async Task Country_AddNewCountry_ShouldHadFailEndReturnNotifications()
        {
            //Arrange
            var countryViewModel = new CountryViewModel();

            //Act
            var postResponse = await _integrationTestFixture.Client.PostAsJsonAsync(_requestAdd, countryViewModel);
            var jsonResponse = await postResponse.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<CountryDeserialize>(jsonResponse);

            //Assert
            Assert.False(obj.Success);
            Assert.True(obj.Errors.Any());
            Assert.Contains(ConstantMessages.CountryNameRequired_PT, obj.Errors);
        }

        [Fact(DisplayName = "Get all countries with success"), Priority(3)]
        [Trait("Categoria", "País - Integração")]
        public async Task Country_GetAllCountries_ShouldReturnWithSuccess()
        {
            //Arrange & act
            var response = await _integrationTestFixture.Client.GetAsync(_requestGetAll);
            string jsonCountries = await response.Content.ReadAsStringAsync();

            var countriesViewModel = JsonConvert.DeserializeObject<CountriesDeserialize>(jsonCountries);
            int index = new Random().Next(1, countriesViewModel.Data.Count);

            var countryViewModel = countriesViewModel.Data.ElementAt(index);
            _integrationTestFixture.CountryId = countryViewModel.Id.GetValueOrDefault();
            //Assert
            Assert.NotEmpty(jsonCountries);
        }

        [Fact(DisplayName = "Get country by id with success"), Priority(4)]
        [Trait("Categoria", "País - Integração")]
        public async Task Country_GetCountryById_ShouldReturnWithSuccess()
        {
            //Arrange & act
            var response = await _integrationTestFixture.Client.GetAsync($"{_requestGetById}{_integrationTestFixture.CountryId}");
            var jsonCountry = await response.Content.ReadAsStringAsync();

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Update country with success"), Priority(5)]
        [Trait("Categoria", "País - Integração")]
        public async Task Country_UpdateCountry_ShouldUpdatedWithSuccess()
        {
            //Arrange
            var response = await _integrationTestFixture
                                    .Client
                                    .GetAsync($"{_requestGetById}{_integrationTestFixture.CountryId}");

            var jsonCountry = await response.Content.ReadAsStringAsync();
            var countryViewModel = JsonConvert.DeserializeObject<CountryDeserialize>(jsonCountry)?.Data;
            response.EnsureSuccessStatusCode();

            countryViewModel.Name = "Nome Alterado";

            //Act
            var responseUpdate = await _integrationTestFixture
                                            .Client
                                            .PutAsJsonAsync($"{_requestUpdate}{countryViewModel.Id}", countryViewModel);

            //Assert
            responseUpdate.EnsureSuccessStatusCode();
            var responseUpdated = await _integrationTestFixture
                                                .Client
                                                .GetAsync($"{_requestGetById}{countryViewModel.Id}");
            var jsonCountryUpdated = await responseUpdated.Content.ReadAsStringAsync();
            var countryUpdatedViewModel = JsonConvert.DeserializeObject<CountryDeserialize>(jsonCountryUpdated)?.Data;

            Assert.Equal(countryViewModel.Id.GetValueOrDefault(), countryUpdatedViewModel.Id.GetValueOrDefault());
            Assert.Equal(countryViewModel.Name, countryUpdatedViewModel.Name);
        }

        [Fact(DisplayName = "Update country with fail"), Priority(6)]
        [Trait("Categoria", "País - Integração")]
        public async Task Country_UpdateCountry_ShouldUpdatedWithFailEndReturnNotifications()
        {
            //Arrange
            var countryViewModel = new CountryViewModel { Id = _integrationTestFixture.CountryId, Name = "a" };

            //Act
            var responseUpdate = await _integrationTestFixture
                                            .Client
                                            .PutAsJsonAsync($"{_requestUpdate}{countryViewModel.Id}", countryViewModel);
            var jsonCountryUpdated = await responseUpdate.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<CountryDeserialize>(jsonCountryUpdated);

            //Assert

            Assert.False(obj.Success);
            Assert.True(obj.Errors.Any());
            Assert.Contains(ConstantMessages.CountryNameLengh_PT, obj.Errors);
        }

        [Fact(DisplayName = "Remove country with success"), Priority(7)]
        [Trait("Categoria", "País - Integração")]
        public async Task Country_RemoveCountry_ShouldHadSuccess()
        {
            //Arrange
            var response = await _integrationTestFixture.Client.GetAsync($"{_requestGetById}{_integrationTestFixture.CountryId}");
            var jsonCountry = await response.Content.ReadAsStringAsync();
            var countryViewModel = JsonConvert.DeserializeObject<CountryDeserialize>(jsonCountry)?.Data;

            //Act
            var responseDelete = await _integrationTestFixture.Client.DeleteAsync($"{_requestDelete}{countryViewModel.Id}");
            responseDelete.EnsureSuccessStatusCode();
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

        [JsonProperty("errors")]
        public List<string> Errors { get; set; }
    }
}