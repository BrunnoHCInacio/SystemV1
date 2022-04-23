using API2;
using System.Collections.Generic;
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
    public class ProviderApiTest
    {
        private readonly IntegrationTestFixture<StartupApiTests> _integrationTestFixture;

        private string _requestAdd => "api/provider/Add";
        private string _requestGetAll => "api/provider/GetAll";
        private string _requestGetById => "api/provider/GetById/";

        public ProviderApiTest(IntegrationTestFixture<StartupApiTests> integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
        }

        [Fact(DisplayName = "Add new provider with success"), Priority(1)]
        [Trait("Categoria", "Fornecedor - Integração")]
        public async Task Add_AddNewProvider_MustAddWithSuccess()
        {
            //Arrange
            List<ProviderViewModel> providers = await GenerateProviders(1, 1, 1);

            //Act
            foreach (var provider in providers)
            {
                var response = await _integrationTestFixture.Client.PostAsJsonAsync(_requestAdd, provider);

                //Assert 
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact(DisplayName = "Add new invalid provider"), Priority(1)]
        [Trait("Categoria", "Fornecedor - Integração")]
        public async Task Add_AddNewInvalidProvider_MustNotAddAndReturnMessageNotification()
        {
            //Arrange
            var providerViewModel = new ProviderViewModel();

            //Act
            var response = await _integrationTestFixture.Client.PostAsJsonAsync(_requestAdd, providerViewModel);
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<ProviderViewModel>>(response);

            //Assert
            Assert.False(responseDeserialized.Success);
        }

        [Fact(DisplayName = "Add new valid provider with invalid contact"), Priority(1)]
        [Trait("Categoria", "Fornecedor - Integração")]
        public async Task Add_AddNewValidProviderWithInvalidContact_MustNotAddAndReturnMessageNotification()
        {
            var providers = await GenerateProviders(1, 0, 1, true, false);

            foreach (var provider in providers)
            {
                var response = await _integrationTestFixture.Client.PostAsJsonAsync(_requestAdd, provider);
                var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<ProviderViewModel>>(response);

                Assert.False(responseDeserialized.Success);
            }
        }

        [Fact(DisplayName = "Add new valid provider with invalid address"), Priority(1)]
        [Trait("Categoria", "Fornecedor - Integração")]
        public async Task Add_AddNewValidProviderWithInvalidAddress_MustNotAddAndReturnMessageNotification()
        {
            var providers = await GenerateProviders(1, 1, 0, false);

            foreach (var provider in providers)
            {
                var response = await _integrationTestFixture.Client.PostAsJsonAsync(_requestAdd, provider);
                var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<ProviderViewModel>>(response);

                Assert.False(responseDeserialized.Success);
            }
        }

        [Fact(DisplayName = "Get all providers"), Priority(2)]
        [Trait("Categoria", "Fornecedor - Integração")]
        public async Task GetAll_GetAllProviders_MustGetAllWithSuccess()
        {
            //Arrange
            var page = 1;
            var pageSize = 10;

            //Act
            var response = await _integrationTestFixture.Client.GetAsync(_requestGetAll + $"?page={page}&pageSize={pageSize}");
            var responseDeserialized = await TestTools.DeserializeResponseAsync<DeserializeList<ProviderViewModel>>(response);

            //Assert
            Assert.NotEmpty(responseDeserialized.Data);
            var provider = TestTools.GetRandomEntityInList(responseDeserialized.Data);
            _integrationTestFixture.ProviderId = provider.Id;
        }

        [Fact(DisplayName = "Get provider by id"), Priority(2)]
        [Trait("Categoria", "Fornecedor - Integração")]
        public async Task GetById_GetProviderById_MustReturnProvider()
        {
            //Arrange and act
            var response = await _integrationTestFixture.Client.GetAsync(_requestGetById + _integrationTestFixture.ProviderId);
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<ProviderViewModel>>(response);

            //Assert
            Assert.NotNull(responseDeserialized.Data);
            Assert.NotEmpty(responseDeserialized.Data.Addresses);
            Assert.NotEmpty(responseDeserialized.Data.Contacts);
        }

        #region Private Methods
        private async Task<List<ProviderViewModel>> GenerateProviders(int qty,
                                                                      int qtyAddress,
                                                                      int qtyContact,
                                                                      bool isValidAddress = true,
                                                                      bool isValidContact = true)
        {
            var cityApiTest = new CityApiTest(_integrationTestFixture);
            await cityApiTest.AddCityAsync(20);
            var cities = await cityApiTest.GetCitiesAsync();
            var providerFixture = new ProviderTestFixture();
            var providers = providerFixture.GenerateValidProviderViewModel(qty,
                                                                           cities,
                                                                           qtyAddress,
                                                                           qtyContact,
                                                                           isValidAddress,
                                                                           isValidContact);

            return providers;
        }

        #endregion
    }
}
