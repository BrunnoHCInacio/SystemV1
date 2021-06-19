using API2;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

        [Fact(DisplayName = "Add new state with success")]
        [Trait("Categoria", "Integração - Estado")]
        public async Task State_AddNewState_ShoulHaveSuccess()
        {
            //Arrange
            var stateFixture = new StateTestFixture();
            var state = stateFixture.GenerateValidStateViewModel();

            //Act
            var postResponse = await _integrationTestFixture.Client.PostAsJsonAsync("api/state/Add", state);

            //Assert
            postResponse.EnsureSuccessStatusCode();
        }
    }
}