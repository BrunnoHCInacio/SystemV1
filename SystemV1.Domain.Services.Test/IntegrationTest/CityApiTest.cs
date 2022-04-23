using API2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Test.Fixture;
using SystemV1.Domain.Test.IntegrationTest.Config;
using Xunit;
using Xunit.Priority;

namespace SystemV1.Domain.Test.IntegrationTest
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    [Collection(nameof(IntegrationApiTestFixtureCollection))]
    public class CityApiTest
    {
        private readonly IntegrationTestFixture<StartupApiTests> _integrationTestFixture;
        private readonly StateApiTests _stateApiTests;

        public CityApiTest(IntegrationTestFixture<StartupApiTests> integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
            _stateApiTests = new StateApiTests(_integrationTestFixture);
        }

        private string _requestAdd => "api/city/Add";
        private string _requestUpdate => "api/city/Update/";
        private string _requestDelete => "api/city/Delete/";
        private string _requestGetAll => "api/city/GetAll";
        private string _requestGetById = "api/city/GetById/";

        #region Function Add

        [Fact(DisplayName ="Add new city with success"), Priority(1)]
        [Trait("Categoria", "Cidade - Integração")]
        public async Task Add_AddNewCity_MustAddWithSuccess()
        {
            //Arrange, act and assert
            await AddCityAsync(10);
        }

        [Fact(DisplayName = "Add new invalid city"), Priority(1)]
        [Trait("Categoria", "Cidade - Integração")]
        public async Task Add_AddNewInvalidCity_MustNotAddAndReturnMessageNotification()
        {
            //Arrange
            var cityTestFixture = new CityTestFixture();
            var cityViewModel = cityTestFixture.GenerateInvalidCityViewModel();
            
            //Act 
            var response = await _integrationTestFixture.Client.PostAsJsonAsync(_requestAdd, cityViewModel);
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<CityViewModel>>(response);

            //Assert
            Assert.False(responseDeserialized.Success);
        }
        #endregion

        #region Function Get
        [Fact(DisplayName = "Get all cities")]
        [Trait("Categoria", "Cidade - Integração"), Priority(2)]
        public async Task GetAll_GetAllCities_MustReturnListOfTheCities()
        {
            //Arrange and act
            var cities = await GetCitiesAsync();
            var city = TestTools.GetRandomEntityInList<CityViewModel>(cities);

            //Assert
            Assert.NotNull(city);
            _integrationTestFixture.CityId = city.Id;
        }

        [Fact(DisplayName = "Get all withou page and page size")]
        [Trait("Categoria", "Cidade - Integração"), Priority(2)]
        public async Task GetAll_GetAllCitiesWithOutPageAndPageSize_MustNotReturnCities()
        {
            //Arrange
            var page = 0;
            var pageSize = 0;

            //Act
            var response = await _integrationTestFixture.Client.GetAsync(_requestGetAll + $"?page={page}&pageSize={pageSize}");
            var responseDeserialized = await TestTools.DeserializeResponseAsync<DeserializeList<CityViewModel>>(response);

            //Assert
            Assert.False(responseDeserialized.Success);
            Assert.True(responseDeserialized.Errors.Any());
        }

        [Fact(DisplayName = "Get city by id")]
        [Trait("Categoria", "Cidade - Integração"), Priority(3)]
        public async Task GetById_GetCityById_MustReturnCity()
        {
            //Arrange and act
            var city = await GetCityByIdAsync(_integrationTestFixture.CityId);

            //Assert
            Assert.NotNull(city);
            Assert.NotNull(city.StateId);
            Assert.Equal(city.Id, _integrationTestFixture.CityId);
            Assert.False(string.IsNullOrWhiteSpace(city.StateName));
        }

        public async Task GetByName_GetCityByName_MustReturnListOfTheCity()
        {

        }
        #endregion

        #region Function Update
        [Fact(DisplayName = "Update city with success")]
        [Trait("Categoria", "Cidade - Integração"), Priority(4)]
        public async Task Update_UpdateCityValid_MustUpdateWithSuccess()
        {
            //Arrange
            var alteracao = " Alterado";
            var city = await GetCityByIdAsync(_integrationTestFixture.CityId);
            city.Name += alteracao;

            //Act
            var response = await _integrationTestFixture.Client.PutAsJsonAsync(_requestUpdate + city.Id, city);
            var cityUpdated = await GetCityByIdAsync(_integrationTestFixture.CityId);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains(alteracao, cityUpdated.Name);
        }
        #endregion 

        #region Function Delete
        [Fact(DisplayName = "Delete city with success")]
        [Trait("Categoria", "Cidade - Integração"), Priority(5)]
        public async Task Delete_DeleteCity_MustDeleteWithSuccess()
        {
            //Arrange
            var city = await GetCityByIdAsync(_integrationTestFixture.CityId);

            //Act
            var response = await _integrationTestFixture.Client.DeleteAsync(_requestDelete + city.Id);

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Delete invalid city")]
        [Trait("Categoria", "Cidade - Integração"), Priority(5)]
        public async Task Delete_DeleteCity_MustNotDeleteAndReturnMessageNotification()
        {
            //Arrange and Act
            var response = await _integrationTestFixture.Client.DeleteAsync(_requestDelete + Guid.NewGuid());
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<CityTestFixture>>(response);

            //Assert
            Assert.False(responseDeserialized.Success);
            Assert.True(responseDeserialized.Errors.Any());
        }

        #endregion

        internal async Task<List<CityViewModel>> GetCitiesAsync()
        {
            //Arrange
            var page = 1;
            var pageSize = 10;

            //Act
            var response = await _integrationTestFixture.Client.GetAsync(_requestGetAll + $"?page={page}&pageSize={pageSize}");
            var responseDeserialized = await TestTools.DeserializeResponseAsync<DeserializeList<CityViewModel>>(response);

            return responseDeserialized.Data;
        }


        internal async Task AddCityAsync(int qty,
                                       bool isValid = true)
        {
            //Arrange
            var cityTestFixture = new CityTestFixture();
            await _stateApiTests.AddNewStateAsync(1);
            var states = await _stateApiTests.GetAllStatesAsync(1, 10);
            var state = states.FirstOrDefault();
            var citiesViewModel = cityTestFixture.GenerateValidCityViewModel(qty, state.Id);

            foreach (var cityViewModel in citiesViewModel)
            {
                //Act
                var response = await _integrationTestFixture.Client.PostAsJsonAsync(_requestAdd, cityViewModel);

                //Assert
                if (isValid)
                {
                    response.EnsureSuccessStatusCode();
                }
                else
                {
                    var responseDeserialized = TestTools.DeserializeResponseAsync<Deserialize<CityViewModel>>(response);
                }
            }
        }

        #region Private methods
        private async Task<CityViewModel> GetCityByIdAsync(Guid cityId)
        {
            var response = await _integrationTestFixture.Client.GetAsync(_requestGetById + cityId);
            var cityDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<CityViewModel>>(response);
            return cityDeserialized.Data;
        }
        #endregion
    }
}
