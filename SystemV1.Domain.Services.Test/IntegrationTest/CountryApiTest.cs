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
        private string _requestGetByName => "api/Country/GetByName?name=";
        private string _requestUpdate => "api/country/Update/";
        private string _requestDelete => "api/country/Delete/";

        public CountryApiTest(IntegrationTestFixture<StartupApiTests> integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
        }

        #region Function Add

        [Fact(DisplayName = "Add new country with success"), Priority(1)]
        [Trait("Categoria", "País - Integração")]
        public async Task Country_AddNewCountry_ShouldHaveSuccess()
        {
            //Arrange, act and assert
            await AddNewCountriesAsync(1);
        }

        [Fact(DisplayName = "Add new country with states with success"), Priority(1)]
        [Trait("Categoria","País - Integração")]
        public async Task Add_AddCountryWithStates_ShouldAddCountryAndAddStates()
        {
            //Arrange, act and assert
            await AddNewCountriesAsync(10, true,3);
        }

        [Fact(DisplayName = "Add new country with invalid states"), Priority(1)]
        [Trait("Categoria", "País - Integração")]
        public async Task Add_AddValidCountryWithInvalidStates_ShouldNotAddAndReturnNotifications()
        {
            //Arrange
            var countryFixture = new CountryTestFixture();
            var countryViewModel = countryFixture.GenerateCountryViewModel(1, true, true, 2).FirstOrDefault();

            //act
            var postResponse = await _integrationTestFixture.Client.PostAsJsonAsync(_requestAdd, countryViewModel);
            var jsonResponse = await postResponse.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<Deserialize<CountryViewModel>>(jsonResponse);

            //Assert
            Assert.False(obj.Success);
            Assert.True(obj.Errors.Any());
            Assert.Contains(ConstantMessages.StateNameRequired_Pt, obj.Errors);
        }

        [Fact(DisplayName = "Add new Country with fail"), Priority(1)]
        [Trait("Categoria", "País - Integração")]
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

        #endregion

        #region Function Get

        [Fact(DisplayName = "Get all countries with success"), Priority(2)]
        [Trait("Categoria", "País - Integração")]
        public async Task GetAllCountries_Add10NewCountriesAndGetAll_ShouldAddAndReturnAllActiveCountriesWithSuccess()
        {
            //Arrange and Act
            var countriesViewModel = await GetAllCountriesAsync(1, 10);
            var countryViewModel = countriesViewModel.Data.FirstOrDefault();
            
            _integrationTestFixture.CountryId = countryViewModel.Id.GetValueOrDefault();
            
            //Assert
            Assert.True(countriesViewModel.Data.Any());
        }


        [Fact(DisplayName = "Get all countries with states with success"), Priority(2)]
        [Trait("Categoria", "País - Integração")]
        public async Task GetAllCountries_AddNewCountriesAndGetAllValidCountriesWithValidStates_ShouldReturnAllCountriesWithStates()
        {
            //Arrange and Act
            var countriesViewModel = await GetAllCountriesAsync(1, 10);

            //Assert
            Assert.True(countriesViewModel.Data.Any());
            foreach (var country in countriesViewModel.Data)
            {
                Assert.True(country.States.Any());
            };
        }

        [Theory(DisplayName = "Get all countries with pagination"), Priority(2)]
        [Trait("Categoria", "País - Integração")]
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
                var countriesViewModel = await GetAllCountriesAsync(i, qtyItemsPage);

                //Assert
                Assert.Equal(qtyItemsPage, countriesViewModel.Data.Count);
                countriesViewModel.Data.ForEach(c => Assert.DoesNotContain(c, countriesRegistred));

                countriesRegistred.AddRange(countriesViewModel.Data);
            }
        }

        [Fact(DisplayName = "Get all countries with invalid page and page size"), Priority(2)]
        [Trait("Categoria", "País - Integração")]
        public async Task GetAllCountries_GetAllWithInvalidPageAndPageSize_ShouldNotReturnCountriesAndReturnNotifications()
        {
            //Arrange and act
            var response = await _integrationTestFixture.Client.GetAsync(GetRequestToGetAll(0, 0));
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<DeserializeList<CountryViewModel>>(jsonResponse);

            //Assert
            Assert.True(obj.Errors.Any());
            Assert.Contains(ConstantMessages.pageAndPageSizeRequired, obj.Errors);
        }

        [Fact(DisplayName = "Get country by id with success"), Priority(3)]
        [Trait("Categoria", "País - Integração")]
        public async Task GetById_GetCountryById_ShouldReturnWithSuccess()
        {
            //Arrange and Act
            var countryViewModel = await GetCountryByIdAsync(_integrationTestFixture.CountryId);

            //Assert
            Assert.NotNull(countryViewModel);
            Assert.Equal(countryViewModel.Id, _integrationTestFixture.CountryId);
        }

        [Fact(DisplayName = "Get country by name with success"), Priority(3)]
        [Trait("Categoria", "País - Integração")]
        public async Task GetByName_GetCountriesByName_ShouldReturnWithSuccess()
        {
            //Arrange and act
            var countries = await GetAllCountriesAsync(1, 100);
            var index = new Random().Next(countries.Data.Count());
            string name = countries.Data[index].Name;
            var response = await _integrationTestFixture.Client.GetAsync(_requestGetByName + name);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var obj = DeserializeJson<DeserializeList<CountryViewModel>>(jsonResponse);

            //Assert
            Assert.True(obj.Data.Any());
            Assert.Contains(name, obj.Data.Select(d => d.Name));
        }

        #endregion

        #region Function Update

        [Fact(DisplayName = "Update country with success"), Priority(4)]
        [Trait("Categoria", "País - Integração")]
        public async Task Update_UpdateValidCountry_ShouldUpdatedWithSuccess()
        {
            //Arrange
            var country = await GetCountryByIdAsync(_integrationTestFixture.CountryId);
            country.Name += " Nome Alterado";

            //Act
            await UpdateCountryAsync(country, true);

            //Assert
            var countryUpdated = await GetCountryByIdAsync(country.Id.GetValueOrDefault());
            CompareObjectCountry(country, countryUpdated);
        }

        [Fact(DisplayName = "Update country with fail"), Priority(4)]
        [Trait("Categoria", "País - Integração")]
        public async Task Update_UpdateInvalidCountry_ShouldUpdatedWithFailEndReturnNotifications()
        {
            //Arrange
            var countryViewModel = new CountryViewModel 
            { 
                Id = _integrationTestFixture.CountryId, 
                Name = "a" 
            };

            //Act
            var returnUpdate = await UpdateCountryAsync(countryViewModel);

            //Assert
            Assert.False(returnUpdate.Success);
            Assert.True(returnUpdate.Errors.Any());
            Assert.Contains(ConstantMessages.CountryNameLengh_PT, returnUpdate.Errors);
        }

        [Fact(DisplayName = "Update country and update list of the states"), Priority(4)]
        [Trait("Categoria", "País - Integração")]
        public async Task Update_UpdateCountryWithStates()
        {
            //Arrange
            var alteracao = " objeto alterado";
            var country = await GetCountryByIdAsync(_integrationTestFixture.CountryId);

            country.Name += alteracao;

            foreach (var state in country.States)
            {
                state.Name += alteracao;
            }

            //Act
            await UpdateCountryAsync(country);

            //Assert
            var countryUpdated = await GetCountryByIdAsync(country.Id.GetValueOrDefault());
            CompareObjectCountry(country, countryUpdated);
            Assert.Contains(alteracao, countryUpdated.Name);

            foreach (var state in countryUpdated.States)
            {
                Assert.Contains(alteracao, state.Name);
            }
        }
        #endregion

        #region Function Remove

        [Fact(DisplayName = "Remove country with success"), Priority(5)]
        [Trait("Categoria", "País - Integração")]
        public async Task Country_RemoveCountry_ShouldHadSuccess()
        {
            var country = await GetCountryByIdAsync(_integrationTestFixture.CountryId);

            //Arrange and Act
            var responseDelete = await _integrationTestFixture.Client.DeleteAsync($"{_requestDelete}{country.Id}");

            //Assert
            responseDelete.EnsureSuccessStatusCode();

            var countryDeleted = await GetCountryByIdAsync(_integrationTestFixture.CountryId);
            Assert.Null(countryDeleted);

            if (country.States.Any())
            { 
                foreach (var state in country.States)
                {
                    var stateDeleted = await GetStateByIdAsync(state.Id);
                    Assert.Null(stateDeleted);
                };
            }
        }

        [Fact(DisplayName = "Remove invalid country with fail"), Priority(5)]
        [Trait("Categoria", "País - Integração")]
        public async Task Remove_RemoveInvalidCountry_ShouldNotRemoveAndNotifyErrors()
        {
            //Arrange and Act
            var responseDelete = await _integrationTestFixture.Client.DeleteAsync($"{_requestDelete}{Guid.NewGuid()}");

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, responseDelete.StatusCode);
        }

        #endregion

        #region Private methods

        internal async Task AddNewCountriesAsync(int qty, 
                                                bool withState = false,
                                                int qtyOfState = 0)
        {
            //Arrange
            var countryFixture = new CountryTestFixture();
            var countriesViewModel = countryFixture.GenerateCountryViewModel(qty, withState, false, qtyOfState);

            foreach (var countryViewModel in countriesViewModel)
            {
                //Act
                var postResponse = await _integrationTestFixture.Client.PostAsJsonAsync(_requestAdd, countryViewModel);

                //Assert
                postResponse.EnsureSuccessStatusCode();
            }
        }

        private async Task<DeserializeList<CountryViewModel>> GetAllCountriesAsync(int page, int pageSize)
        {
            var response = await _integrationTestFixture.Client.GetAsync(GetRequestToGetAll(page, pageSize));
            response.EnsureSuccessStatusCode();
            string jsonCountries = await response.Content.ReadAsStringAsync();
            return DeserializeJson<DeserializeList<CountryViewModel>>(jsonCountries);
        }

        private async Task<CountryViewModel> GetCountryByIdAsync(Guid id)
        {
             var country = await GetByIdAsync<Deserialize<CountryViewModel>>(id);
            return country?.Data;
        }

        private async Task<StateViewModel> GetStateByIdAsync(Guid id)
        {
            var state = await GetByIdAsync<Deserialize<StateViewModel>>(id);
            return state?.Data;
        }

        private async Task<TViewModel> GetByIdAsync<TViewModel>(Guid id)
        {
            var response = await _integrationTestFixture.Client.GetAsync($"{_requestGetById}{id}");
            response.EnsureSuccessStatusCode();
            var jsonCountry = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TViewModel>(jsonCountry);
        }

        private string GetRequestToGetAll(int page, int pageSize)
        {
            return $"api/Country/GetAll?page={page}&pageSize={pageSize}";
        }

        private TObject DeserializeJson<TObject>(string jsonCountries)
        {
            return JsonConvert.DeserializeObject<TObject>(jsonCountries);
        }

        private async Task<Deserialize<CountryViewModel>> UpdateCountryAsync(CountryViewModel countryViewModel, 
                                                                  bool verifyStateCode = false)
        {
            var responseUpdate = await _integrationTestFixture
                                            .Client
                                            .PutAsJsonAsync($"{_requestUpdate}{countryViewModel.Id}", countryViewModel);
            
            if (verifyStateCode)
            {
                responseUpdate.EnsureSuccessStatusCode();
            }

            var jsonCountryUpdated = await responseUpdate.Content.ReadAsStringAsync();
            return DeserializeJson<Deserialize<CountryViewModel>>(jsonCountryUpdated);
        }

        private static void CompareObjectCountry(CountryViewModel country, 
                                                 CountryViewModel countryUpdated)
        {
            Assert.Equal(country.Id.GetValueOrDefault(), countryUpdated.Id.GetValueOrDefault());
            Assert.Equal(country.Name, countryUpdated.Name);

            if(country.States != null)
            {
                Assert.True(country.States.Any());
            }
        }
        #endregion
    }
}