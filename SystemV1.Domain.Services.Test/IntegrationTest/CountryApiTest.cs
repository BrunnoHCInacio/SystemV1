using API2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        
        private string _requestGetById => "api/Country/GetById/";
        private string _requestUpdate => "api/country/Update/";
        private string _requestDelete => "api/country/Delete/";

        public CountryApiTest(IntegrationTestFixture<StartupApiTests> integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
        }

        [Fact(DisplayName = "Add new country with success")]
        [Trait("Categoria", "País - Integração")]
        public async Task Country_AddNewCountry_ShouldHaveSuccess()
        {
            //Arrange, act and assert
            await AddNewCountriesAsync(1);
        }

        public async Task Add_AddCountryWithStates_ShouldAddCountryAndAddStates()
        {
            //Arrange

        }

        [Fact(DisplayName = "Add new Country with fail")]
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

        [Fact(DisplayName = "Get all countries with success")]
        [Trait("Categoria", "País - Integração")]
        public async Task GetAllCountries_Add10NewCountriesAndGetAll_ShouldAddAndReturnAllActiveCountriesWithSuccess()
        {
            //Arrange & act
            await AddNewCountriesAsync(10);

            var response = await _integrationTestFixture.Client.GetAsync(GetRequestToGetAll(1,10));
            string jsonCountries = await response.Content.ReadAsStringAsync();

            var countriesViewModel = JsonConvert.DeserializeObject<CountriesDeserialize>(jsonCountries);
        
            //Assert
            Assert.Equal(10, countriesViewModel.Data.Count);
            Assert.NotEmpty(jsonCountries);
        }

        [Theory(DisplayName ="Get all countries with pagination")]
        [Trait("Categoria", "País - Integração")]
        [InlineData(50,10)]
        [InlineData(100,10)]
        [InlineData(1000,5)]
        public async Task GetAllCountries_AddNewCountriesAndGetAllWithPagination_ShouldAddAllTheCountriesAndGetItWithPagination(int qtyCuntries,int qtyItemsPage)
        {
            //Arrage
            await AddNewCountriesAsync(qtyCuntries);
            int qtyPages = qtyCuntries / qtyItemsPage;
            var countriesRegistred = new List<CountryViewModel>();

            for (int i = 1; i <= qtyPages; i++)
            {
                //Act
                var countriesViewModel = await GetAllCountriesAsync(i,qtyItemsPage);

                //Assert
                Assert.Equal(qtyItemsPage,countriesViewModel.Data.Count);
                countriesViewModel.Data.ForEach(c=> Assert.DoesNotContain(c,countriesRegistred));

                countriesRegistred.AddRange(countriesViewModel.Data);
            }
        }

        [Fact(DisplayName = "Get country by id with success")]
        [Trait("Categoria", "País - Integração")]
        public async Task Country_GetCountryById_ShouldReturnWithSuccess()
        {
            //Arrange
            await AddNewCountriesAsync(1);
            var countriesViewModel = await GetAllCountriesAsync(1, 1);
            var country = countriesViewModel.Data.FirstOrDefault();

            //Act
            var countryViewModel = await GetCountryByIdAsync(country.Id.GetValueOrDefault());

            //Assert
            Assert.NotNull(countryViewModel.Data);
            Assert.Equal(country.Id, countryViewModel.Data.Id);
            Assert.Equal(country.Name, countryViewModel.Data.Name);   
        }

        [Fact(DisplayName = "Update country with success")]
        [Trait("Categoria", "País - Integração")]
        public async Task Update_UpdateCountry_ShouldUpdatedWithSuccess()
        {
            //Arrange
            await AddNewCountriesAsync(1);
            var countries = await GetAllCountriesAsync(1, 1);
            var country = countries.Data.FirstOrDefault();

          
            country.Name += " Nome Alterado";

            //Act
            var responseUpdate = await _integrationTestFixture
                                            .Client
                                            .PutAsJsonAsync($"{_requestUpdate}{country.Id}", country);

            //Assert
            var countryUpdated = await GetCountryByIdAsync(country.Id.GetValueOrDefault());

            Assert.Equal(country.Id.GetValueOrDefault(), countryUpdated.Data.Id.GetValueOrDefault());
            Assert.Equal(country.Name, countryUpdated.Data.Name);
        }

        [Fact(DisplayName = "Update country with fail"), Priority(6)]
        [Trait("Categoria", "País - Integração")]
        public async Task Country_UpdateCountry_ShouldUpdatedWithFailEndReturnNotifications()
        {
            await AddNewCountriesAsync(1);
            var countries = await GetAllCountriesAsync(1, 1);
            var country = countries.Data.FirstOrDefault();

            //Arrange
            var countryViewModel = new CountryViewModel { Id = country.Id, Name = "a" };

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
            await AddNewCountriesAsync(1);
            var countries = await GetAllCountriesAsync(1, 1);
            var country = countries.Data.FirstOrDefault();
            
            //Act
            var responseDelete = await _integrationTestFixture.Client.DeleteAsync($"{_requestDelete}{country.Id}");
            
            //Assert
            responseDelete.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName ="Remove invalid country with fail")]
        [Trait("Categoria", "País - Integração")]
        public async Task Remove_RemoveInvalidCountry_ShouldNotRemoveAndNotifyErrors()
        {
            //Arrange
            var countryFixture = new CountryTestFixture();
            var country = countryFixture.GenerateValidContryViewModel();

            //Act
            var responseDelete = await _integrationTestFixture.Client.DeleteAsync($"{_requestDelete}{country.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, responseDelete.StatusCode);
        }

        private async Task AddNewCountriesAsync(int qtd)
        {
            //Arrange
            var countryFixture = new CountryTestFixture();

            for (int i = 0; i < qtd; i++)
            {
                var countryViewModel = countryFixture.GenerateValidContryViewModel();

                //Act
                var postResponse = await _integrationTestFixture.Client.PostAsJsonAsync(_requestAdd, countryViewModel);

                //Assert
                postResponse.EnsureSuccessStatusCode();
            }
        }

        private async Task<CountriesDeserialize> GetAllCountriesAsync(int page, int pageSize)
        {
            var response = await _integrationTestFixture.Client.GetAsync(GetRequestToGetAll(page, pageSize));
            response.EnsureSuccessStatusCode();
            string jsonCountries = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CountriesDeserialize>(jsonCountries);
        }

        private async Task<CountryDeserialize> GetCountryByIdAsync(Guid id)
        {
            var response = await _integrationTestFixture.Client.GetAsync($"{_requestGetById}{id}");
            response.EnsureSuccessStatusCode();
            var jsonCountry = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CountryDeserialize>(jsonCountry);
        }

        private string GetRequestToGetAll(int page, int pageSize)
        {
            return $"api/Country/GetAll?page={page}&pageSize={pageSize}";
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