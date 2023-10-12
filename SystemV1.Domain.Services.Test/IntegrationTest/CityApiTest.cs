using API2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    public class CityApiTest : IntegrationTestBase
    {
        //Criar testes para exclusão de cidade que contém em um endereço.

        // Criar testes com o contexto de autorização, para cada tipo de requisição, testes de falha de autenticação.

        private readonly IntegrationTestFixture<StartupApiTests> _integrationTestFixture;
        private readonly StateApiTests _stateApiTests;

        private readonly string _requestAdd = "api/city/Add";
        private readonly string _requestUpdate = "api/city/Update/";
        private readonly string _requestDelete = "api/city/Delete/";
        private readonly string _requestGetAll = "api/city/GetAll";
        private readonly string _requestGetById = "api/city/GetById/";
        private readonly string _requestGetCityStateById = "api/city/GetCityStateById/";

        public CityApiTest(IntegrationTestFixture<StartupApiTests> integrationTestFixture) : base(integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
            _stateApiTests = new StateApiTests(_integrationTestFixture);
        }

        #region Function Add

        [Fact(DisplayName = "Add new city with success"), Priority(1)]
        [Trait("UnitTests - Integration", "City")]
        public async Task Add_AddNewCity_MustAddWithSuccess()
        {
            //Arrange, act and assert
            await AddCityAsync(10);
        }

        [Fact(DisplayName = "Add new invalid city"), Priority(1)]
        [Trait("UnitTests - Integration", "City")]
        public async Task Add_AddNewInvalidCity_MustNotAddAndReturnMessageNotification()
        {
            //Arrange
            var cityTestFixture = new CityTestFixture();
            var cityViewModel = cityTestFixture.GenerateInvalidCityViewModel();

            //Act
            var response = await AddAsync(cityViewModel, _requestAdd, false);
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<CityViewModel>>(response);

            //Assert
            Assert.False(responseDeserialized.Success);
        }

        #endregion Function Add

        #region Function Get

        [Fact(DisplayName = "Get all cities")]
        [Trait("UnitTests - Integration", "City"), Priority(2)]
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
        [Trait("UnitTests - Integration", "City"), Priority(2)]
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
        [Trait("UnitTests - Integration", "City"), Priority(3)]
        public async Task GetById_GetCityById_MustReturnCity()
        {
            //Arrange and act
            var city = await GetCityByIdAsync(_integrationTestFixture.CityId, true);

            //Assert
            Assert.NotNull(city);
            Assert.NotNull(city.StateId);
            Assert.Equal(city.Id, _integrationTestFixture.CityId);
            Assert.False(string.IsNullOrWhiteSpace(city.StateName));
        }

        #endregion Function Get

        #region Function Update

        [Fact(DisplayName = "Update city with success")]
        [Trait("UnitTests - Integration", "City"), Priority(4)]
        public async Task Update_UpdateCityValid_MustUpdateWithSuccess()
        {
            //Arrange
            var alteracao = " Alterado";
            var city = await GetCityByIdAsync(_integrationTestFixture.CityId, true);
            city.Name += alteracao;

            //Act
            await UpdateAsync(city, city.Id, _requestUpdate);
            var cityUpdated = await GetCityByIdAsync(_integrationTestFixture.CityId);

            //Assert
            Assert.Contains(alteracao, cityUpdated.Name);
        }

        [Fact(DisplayName = "Update with invalid city")]
        [Trait("UnitTests - Integration", "City"), Priority(4)]
        public async Task Update_UpdateInvalidCity_MustNotUpdate()
        {
            //Arrange
            var city = await GetCityByIdAsync(_integrationTestFixture.CityId);
            city.Name = "";

            //Act
            var response = await _integrationTestFixture.Client.PutAsJsonAsync(_requestUpdate + city.Id, city);
            var responseDeserialized = TestTools.DeserializeResponseAsync<CityViewModel>(response);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        #endregion Function Update

        #region Function Delete

        [Fact(DisplayName = "Delete city with success")]
        [Trait("UnitTests - Integration", "City"), Priority(5)]
        public async Task Delete_DeleteCity_MustDeleteWithSuccess()
        {
            //Arrange
            var city = await GetCityByIdAsync(_integrationTestFixture.CityId);

            //Act
            var response = await RemoveAsync(city.Id, _requestDelete);

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Delete invalid city")]
        [Trait("UnitTests - Integration", "City"), Priority(5)]
        public async Task Delete_DeleteCity_MustNotDeleteAndReturnMessageNotification()
        {
            //Arrange and Act
            var response = await RemoveAsync(Guid.NewGuid(), _requestDelete);
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<CityTestFixture>>(response);

            //Assert
            Assert.False(responseDeserialized.Success);
            Assert.True(responseDeserialized.Errors.Any());
        }

        #endregion Function Delete

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

        private async Task<CityViewModel> GetCityByIdAsync(Guid cityId, bool withState = false)
        {
            var url = "";
            if (withState)
            {
                url = _requestGetCityStateById + cityId;
            }
            else
            {
                url = _requestGetById + cityId;
            }
            var response = await _integrationTestFixture.Client.GetAsync(url);
            var cityDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<CityViewModel>>(response);
            return cityDeserialized.Data;
        }

        #endregion Private methods
    }
}