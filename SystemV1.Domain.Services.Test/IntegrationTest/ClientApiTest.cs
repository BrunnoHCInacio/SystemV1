using API2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
        private string _requestGetById => "api/client/GetById/";
        private string _requestGetByName => "api/client";
        private string _requestUpdate => "api/client/Update/";
        private string _requestRemove => "api/client/Remove/";

        private string _requestCityGetAll => "api/city/GetAll";

        public ClientApiTest(IntegrationTestFixture<StartupApiTests> integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
            var stateApiTest = new CityApiTest(_integrationTestFixture);
            Task.Run(() => stateApiTest.AddCityAsync(20)).Wait();
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
            var cityApiTest = new CityApiTest(_integrationTestFixture);
            var citiesViewModel = await cityApiTest.GetCitiesAsync();
            var clientFixture = new ClientTestFixture();
            var clientsViewModel = clientFixture.GenerateClientViewModel(qtyClients: 1,
                                                                         citiesViewModel: citiesViewModel,
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
        [Fact(DisplayName = "Get all clients with success"), Priority(2)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Client_GetAllClients_ShouldReturnsWithSuccess()
        {
            //Arrange
            var page = 1;
            var pageSize = 10;

            //Act
            var response = await _integrationTestFixture.Client.GetAsync(_requestGetAll + $"?page={page}&pageSize={pageSize}");
            var responseDeserialized = await TestTools.DeserializeResponseAsync<DeserializeList<ClientViewModel>>(response);

            //Assert
            Assert.True(responseDeserialized.Success);
            Assert.True(responseDeserialized.Data.Any());
            var client = TestTools.GetRandomEntityInList(responseDeserialized.Data);
            _integrationTestFixture.ClientId = client.Id;
        }

        [Fact(DisplayName = "Get client by id with success"), Priority(3)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Client_GetById_ShouldHasSuccess()
        {
            //Arrange and Act
            Deserialize<ClientViewModel> responseDeserialized = await GetClientByIdAsync(_integrationTestFixture.ClientId);

            //Assert
            Assert.True(responseDeserialized.Success);
            Assert.NotNull(responseDeserialized.Data);
            Assert.Equal(_integrationTestFixture.ClientId, responseDeserialized.Data.Id);
        }

        #endregion

        #region Function Update
        [Fact(DisplayName = "Update client with success"), Priority(4)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Client_UpdateClient_ShouldHasSuccess()
        {
            //Arrange
            var updateName = " alterado";
            var updateDocument = "56745894025";
            var clientViewModel = await GetClientByIdAsync(_integrationTestFixture.ClientId);
            clientViewModel.Data.Name += updateName;
            clientViewModel.Data.Document = updateDocument;
            //Act
            var response = await _integrationTestFixture.Client.PutAsJsonAsync(_requestUpdate + clientViewModel.Data.Id, clientViewModel.Data);
            response.EnsureSuccessStatusCode();

            var clientUpdatedViewModel = await GetClientByIdAsync(_integrationTestFixture.ClientId);
            
            //Assert
            Assert.Equal(_integrationTestFixture.ClientId, clientViewModel.Data.Id);
            Assert.Contains(updateName, clientUpdatedViewModel.Data.Name);
            var documentWithoutMask = Regex.Replace(updateDocument, @"[^0-9]", string.Empty);
            Assert.Equal(updateDocument, documentWithoutMask);
        }

        [Fact(DisplayName = "Update client, remove addresses"), Priority(4)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Update_UpdateClientRemoveAddresses_MustUpdateWithSuccess()
        {
            //Arrange
            var updateName = " alterado";
            var updateDocument = "56745894025";
            var clientViewModel = await GetClientByIdAsync(_integrationTestFixture.ClientId);
            clientViewModel.Data.Name += updateName;
            clientViewModel.Data.Document = updateDocument;
            clientViewModel.Data.Addresses = null;
            //Act
            var response = await _integrationTestFixture.Client.PutAsJsonAsync(_requestUpdate + clientViewModel.Data.Id, clientViewModel.Data);

            var clientUpdatedViewModel = await GetClientByIdAsync(_integrationTestFixture.ClientId);

            //Assert
            Assert.Equal(_integrationTestFixture.ClientId, clientViewModel.Data.Id);
            Assert.Contains(updateName, clientUpdatedViewModel.Data.Name);
            var documentWithoutMask = Regex.Replace(updateDocument, @"[^0-9]", string.Empty);
            Assert.Equal(updateDocument, documentWithoutMask);
            Assert.False(clientUpdatedViewModel.Data.Addresses.Any());
        }

        [Fact(DisplayName = "Update client, remove addresses"), Priority(4)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Update_UpdateClientRemoveContacts_MustUpdateWithSuccess()
        {
            //Arrange
            var updateName = " alterado";
            var updateDocument = "56745894025";
            var clientViewModel = await GetClientByIdAsync(_integrationTestFixture.ClientId);
            clientViewModel.Data.Name += updateName;
            clientViewModel.Data.Document = updateDocument;
            clientViewModel.Data.Contacts = null;
            //Act
            var response = await _integrationTestFixture.Client.PutAsJsonAsync(_requestUpdate + clientViewModel.Data.Id, clientViewModel.Data);

            var clientUpdatedViewModel = await GetClientByIdAsync(_integrationTestFixture.ClientId);

            //Assert
            Assert.Equal(_integrationTestFixture.ClientId, clientViewModel.Data.Id);
            Assert.Contains(updateName, clientUpdatedViewModel.Data.Name);
            var documentWithoutMask = Regex.Replace(updateDocument, @"[^0-9]", string.Empty);
            Assert.Equal(updateDocument, documentWithoutMask);
            Assert.False(clientUpdatedViewModel.Data.Contacts.Any());
        }

        [Fact(DisplayName = "Update invalid client"), Priority(4)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Update_UpdateInvalidClient_MustNotUpdateAndReturnMessageNotification()
        {
            //Arrange
            var clientViewModel = new ClientViewModel();
            
            //Act
            var response = await _integrationTestFixture.Client.PutAsJsonAsync(_requestUpdate+clientViewModel.Id, clientViewModel);
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<ClientViewModel>>(response);

            //Assert
            Assert.False(responseDeserialized.Success);
            Assert.Contains(ConstantMessages.PeopleNameRequired_PT, responseDeserialized.Errors);
            Assert.Contains(ConstantMessages.ClientDocumentRequired_PT, responseDeserialized.Errors);
        }

        [Fact(DisplayName = "Update client not saved "), Priority(4)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Update_UpdateClientNotSaved_MustNotUpdateAndReturnMessageNotification()
        {
            //Arrange
            var clientFixture = new ClientTestFixture();
            var clientsViewModel = clientFixture.GenerateClientViewModel(1, null);
            var clientViewModel = clientsViewModel.FirstOrDefault();
            
            //Act
            var response = await _integrationTestFixture.Client.PutAsJsonAsync(_requestUpdate + clientViewModel.Id, clientViewModel);
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<ClientViewModel>>(response);

            //Assert
            Assert.False(responseDeserialized.Success);
        }

        [Fact(DisplayName = "Update valid client with invalid address "), Priority(4)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Update_UpdateValidClientWithInvalidAddresses_MustNotUpdateAndReturnMessageNotification()
        {
            //Arrange
            var clientViewModel =await  GetClientByIdAsync(_integrationTestFixture.ClientId);
            clientViewModel.Data.Addresses = new List<AddressViewModel> { new AddressViewModel() };

            //Act
            var response = await _integrationTestFixture.Client.PutAsJsonAsync(_requestUpdate + clientViewModel.Data.Id, clientViewModel.Data);
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<ClientViewModel>>(response);

            //Assert
            Assert.False(responseDeserialized.Success);
            Assert.Contains(ConstantMessages.StreetRequiredt_PT, responseDeserialized.Errors);
            Assert.Contains(ConstantMessages.DistrictRequired_PT, responseDeserialized.Errors);
        }

        [Fact(DisplayName = "Update valid client with invalid address "), Priority(4)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Update_UpdateValidClientWithInvalidContacts_MustNotUpdateAndReturnMessageNotification()
        {
            //Arrange
            var clientViewModel = await GetClientByIdAsync(_integrationTestFixture.ClientId);
            clientViewModel.Data.Contacts = new List<ContactViewModel> { new ContactViewModel() };

            //Act
            var response = await _integrationTestFixture.Client.PutAsJsonAsync(_requestUpdate + clientViewModel.Data.Id, clientViewModel.Data);
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<ClientViewModel>>(response);

            //Assert
            Assert.False(responseDeserialized.Success);
        }
        #endregion

        #region Function Remove
        [Fact(DisplayName = "Remove client with success"), Priority(5)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Client_RemoveClient_MustRemoveWithSuccess()
        {
            //Arrange and Act
            var response = await _integrationTestFixture.Client.DeleteAsync(_requestRemove + _integrationTestFixture.ClientId);

            //Assert
            response.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Remove client with invalid id"), Priority(5)]
        [Trait("Categoria", "Cliente - Integração")]
        public async Task Client_RemoveClient_MustNotRemoveAndReturnMessageNotification()
        {
            //Arrange and Act
            var response = await _integrationTestFixture.Client.DeleteAsync(_requestRemove + Guid.Empty);
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<ClientViewModel>>(response);

            //Assert
            Assert.False(responseDeserialized.Success);
            Assert.Contains(ConstantMessages.ClientNotFound, responseDeserialized.Errors);
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
            var citiesViewModel = await GetCitiesAsync();
            var clientFixture = new ClientTestFixture();
            var clientsViewModel = clientFixture.GenerateClientViewModel(qtyClients,
                                                                         citiesViewModel,
                                                                         qtyAddress,
                                                                         qtyContact,
                                                                         isValidAddress,
                                                                         isValidContact);

            foreach(var client in clientsViewModel)
            {
                //Act
                var postResponse = await _integrationTestFixture.Client.PostAsJsonAsync(_requestAdd, client);
                
                //Assert
                if (isSuccessCase)
                {
                    postResponse.EnsureSuccessStatusCode();
                }
            };
        }

        private async Task<List<CityViewModel>> GetCitiesAsync()
        {
            var response = await _integrationTestFixture.Client.GetAsync(_requestCityGetAll + "?page=1&pageSize=1");
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var stateDeserialized = JsonConvert.DeserializeObject<DeserializeList<CityViewModel>>(jsonResponse);
            return stateDeserialized.Data;
        }

        private async Task<Deserialize<ClientViewModel>> GetClientByIdAsync(Guid id)
        {
            var response = await _integrationTestFixture.Client.GetAsync(_requestGetById + id);
            return await TestTools.DeserializeResponseAsync<Deserialize<ClientViewModel>>(response);
        }
        #endregion
    }
}