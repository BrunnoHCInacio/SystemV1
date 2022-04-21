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
using SystemV1.Domain.Test.Fixture;
using SystemV1.Domain.Test.IntegrationTest.Config;
using Xunit;
using Xunit.Priority;

namespace SystemV1.Domain.Test.IntegrationTest
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    [Collection(nameof(IntegrationApiTestFixtureCollection))]
    public class ClientApiTest
    {
        private readonly IntegrationTestFixture<StartupApiTests> _integrationTestFixture;
        private string _requestAdd => "api/client/Add";
        private string _requestGetAll => "api/client/GetAll";
        private string _requestGetById => "api/client";
        private string _requestGetByName => "api/client";
        private string _requestUpdate => "api/client";
        private string _requestDelete => "api/client";

        private string _requestCityGetAll => "api/city/GetAll";

        public ClientApiTest(IntegrationTestFixture<StartupApiTests> integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
            var stateApiTest = new CityApiTest(_integrationTestFixture);
            Task.Run(() => stateApiTest.AddCityAsync(1)).Wait();
        }

        #region Function Add
        [Fact(DisplayName = "Add new client with success"), Priority(1)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Client_AddNewClient_ShouldHasSuccess()
        {
            //Arrange, act and assert
            await AddNewCLientAsync(qtyClients: 4, 
                                    qtyAddress: 2, 
                                    qtyContact: 5);
        }

        [Fact(DisplayName ="Add new client without address"), Priority(1)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Add_AddNewClientWithoutAddress_ShouldAddWithSuccess()
        {
            await AddNewCLientAsync(1);
        }

        [Fact(DisplayName = "Add new client with invalid address"), Priority(1)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Add_AddNewClientWithIvalidAddress_ShouldNotAddAndReturnNotifications()
        {
            //Arrange
            var cityViewModel = await GetCityAsync();
            var clientFixture = new ClientTestFixture();
            var clientsViewModel = clientFixture.GenerateClientViewModel(qtyClients: 1,
                                                                         cityViewModel: cityViewModel,
                                                                         qtyAddress: 1,
                                                                         isValidAddress: false);

            //and act
            var postResponse = await _integrationTestFixture.Client.PostAsJsonAsync(_requestAdd, clientsViewModel.FirstOrDefault());

            //Assert
            var jsonResponse = await postResponse.Content.ReadAsStringAsync();
            var clientDeserialized = JsonConvert.DeserializeObject<Deserialize<ClientViewModel>>(jsonResponse);
            Assert.False(clientDeserialized.Success);
            Assert.Contains(ConstantMessages.StreetRequiredt_PT, clientDeserialized.Errors);
            Assert.Contains(ConstantMessages.DistrictRequired_PT, clientDeserialized.Errors);
        }
        #endregion

        # region Function Get
        private async Task<CityViewModel> GetCityAsync()
        {
            var response = await _integrationTestFixture.Client.GetAsync(_requestCityGetAll + "?page=1&pageSize=1");
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var stateDeserialized = JsonConvert.DeserializeObject<DeserializeList<CityViewModel>>(jsonResponse);
            return stateDeserialized.Data.FirstOrDefault();
        }

        [Fact(DisplayName = "Get all clients with success"), Priority(2)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Client_GetAllClients_ShouldReturnsWithSuccess()
        {
            //Arrange
            var page = 1;
            var pageSize = 2;

            //Act
            var response = await _integrationTestFixture.Client.GetAsync(_requestGetAll + $"?page={page}&pageSize={pageSize}");
            var responseDeserialized = TestTools.DeserializeResponseAsync<DeserializeList<ClientViewModel>>(response);

            //Assert
        }

        [Fact(DisplayName = "Get client by id with success"), Priority(3)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Client_GetById_ShouldHasSuccess()
        {
            //Arrange
            //Act
            //Assert
        }
        #endregion

        #region Function Update
        [Fact(DisplayName = "Update client with success"), Priority(4)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Client_UpdateClient_ShouldHasSuccess()
        {
            //Arrange
            //Act
            //Assert
        }
        #endregion

        #region Function Remove
        [Fact(DisplayName = "Remove client with success"), Priority(5)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Client_RemoveClient_MustRemoveWithSuccess()
        {
            //Arrange and Act
            _integrationTestFixture.Client.DeleteAsync(_requestDelete + _integrationTestFixture.CLien);
            //Assert
        }
        #endregion

        #region Private Methods

        private async Task AddNewCLientAsync(int qtyClients,
                                             int qtyAddress = 0,
                                             bool isValidAddress = true,
                                             int qtyContact = 0,
                                             bool isValidContact = true,
                                             bool isSuccessCase = true)
        {
            //Arrange
            var cityViewModel = await GetCityAsync();
            var clientFixture = new ClientTestFixture();
            var clientsViewModel = clientFixture.GenerateClientViewModel(qtyClients,
                                                                         cityViewModel,
                                                                         qtyAddress,
                                                                         qtyContact,
                                                                         isValidAddress,
                                                                         isValidContact);

            foreach(var client in clientsViewModel)
            {
                //Act
                var postResponse = await _integrationTestFixture.Client.PostAsJsonAsync(_requestAdd, client);
                var jsonResponse = await postResponse.Content.ReadAsStringAsync();

                //Assert
                if (isSuccessCase)
                {
                    postResponse.EnsureSuccessStatusCode();
                }
            };
        }

        #endregion
    }
}