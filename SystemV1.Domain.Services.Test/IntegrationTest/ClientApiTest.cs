using API2;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
        private string _requestGetAll => "api/client";
        private string _requestGetById => "api/client";
        private string _requestGetByName => "api/client";
        private string _requestUpdate => "api/client";
        private string _requestDelete => "api/client";

        public ClientApiTest(IntegrationTestFixture<StartupApiTests> integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
        }

        [Fact(DisplayName = "Add new client with success"), Priority(1)]
        [Trait("Categoria", "Integração - País")]
        public async Task Client_AddNewClient_ShouldHasSuccess()
        {
            //Arrange
            var clientFixture = new ClientTestFixture();
            var clientViewModel = clientFixture.GenerateClientViewModel(4, 2, 5);

            //Act
            var postResponse = await _integrationTestFixture.Client.PostAsJsonAsync(_requestAdd, clientViewModel);

            //Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Get all clients with success"), Priority(2)]
        [Trait("Categoria", "Integração - País")]
        public async Task Client_GetAllClients_ShouldReturnsWithSuccess()
        {
            //Arrange
            //Act
            //Assert
        }

        [Fact(DisplayName = "Get client by id with success"), Priority(3)]
        [Trait("Categoria", "Integração - País")]
        public async Task Client_GetById_ShouldHasSuccess()
        {
            //Arrange
            //Act
            //Assert
        }

        [Fact(DisplayName = "Update client with success"), Priority(4)]
        [Trait("Categoria", "Integração - País")]
        public async Task Client_UpdateClient_ShouldHasSuccess()
        {
            //Arrange
            //Act
            //Assert
        }

        [Fact(DisplayName = "Remove client with success"), Priority(5)]
        [Trait("Categoria", "Integração - País")]
        public async Task Client_RemoveClient_ShouldHasSuccess()
        {
            //Arrange
            //Act
            //Assert
        }
    }
}