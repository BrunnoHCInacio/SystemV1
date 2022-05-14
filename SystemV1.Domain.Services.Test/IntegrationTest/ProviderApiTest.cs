using API2;
using System;
using System.Collections.Generic;
using System.Net.Http;
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
    public class ProviderApiTest
    {
        private readonly IntegrationTestFixture<StartupApiTests> _integrationTestFixture;

        private string _requestAdd => "api/provider/Add";
        private string _requestGetAll => "api/provider/GetAll";
        private string _requestGetById => "api/provider/GetById/";
        private string _requestUpdate => "api/provider/Update/";
        private string _requestRemove => "api/provider/Remove/";

        public ProviderApiTest(IntegrationTestFixture<StartupApiTests> integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
        }

        #region Function Add
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
        #endregion

        #region Function Get
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

        [Fact(DisplayName = "Get provider by id"), Priority(3)]
        [Trait("Categoria", "Fornecedor - Integração")]
        public async Task GetById_GetProviderById_MustReturnProvider()
        {
            //Arrange and act
            var responseDeserialized = await GetProviderByIdAsync(_integrationTestFixture.ProviderId);

            //Assert
            Assert.NotNull(responseDeserialized.Data);
            Assert.NotEmpty(responseDeserialized.Data.Addresses);
            Assert.NotEmpty(responseDeserialized.Data.Contacts);
        }

        [Fact(DisplayName = "Get provider by id"), Priority(3)]
        [Trait("Categoria", "Fornecedor - Integração")]
        public async Task GetById_GetProviderByInvalidId_MustNotReturnAnyoneProvider()
        {
            //Arrange and act
            var responseDeserialized = await GetProviderByIdAsync(Guid.Empty);

            //Assert
            Assert.Null(responseDeserialized);
        }
        #endregion

        #region Function Update
        [Fact(DisplayName = "Update valid provider with success"), Priority(4)]
        [Trait("Categoria", "Fornecedor - Integração")]
        public async Task Update_UpdateProvider_MustUpdatedOnlyProviderWithSuccess()
        {
            //Arrange
            var updated = " alterado";
            var documentUpdated = "53.606.274/0001-95";
            var providerViewModel = await GetProviderByIdAsync(_integrationTestFixture.ProviderId);

            //Act
            providerViewModel.Data.Name += updated;
            providerViewModel.Data.Document = documentUpdated;
            providerViewModel.Data.Addresses = null;
            providerViewModel.Data.Contacts = null;
            await _integrationTestFixture.Client.PutAsJsonAsync(_requestUpdate + providerViewModel.Data.Id, providerViewModel.Data);
            var providerUpdatedViewModel = await GetProviderByIdAsync(_integrationTestFixture.ProviderId);

            //Assert
            Assert.Equal(_integrationTestFixture.ProviderId, providerUpdatedViewModel.Data.Id);
            Assert.Equal(documentUpdated, providerUpdatedViewModel.Data.Document);
            Assert.Contains(updated, providerUpdatedViewModel.Data.Name);
            Assert.Empty(providerUpdatedViewModel.Data.Addresses);
            Assert.Empty(providerUpdatedViewModel.Data.Contacts);
        }

        [Fact(DisplayName = "Update provider with invalid document"), Priority(4)]
        [Trait("Categoria", "Fornecedor - Integração")]
        public async Task Update_UpdateProviderWithInvalidDocument_MustNotUpdatedAndReturnMessageNotification()
        {
            //Arrange
            var documentUpdated = "448.719.060-60";
            var providerViewModel = await GetProviderByIdAsync(_integrationTestFixture.ProviderId);

            //Act
            providerViewModel.Data.Document = documentUpdated;
            await _integrationTestFixture.Client.PutAsJsonAsync(_requestUpdate + providerViewModel.Data.Id, providerViewModel.Data);
            var providerUpdatedViewModel = await GetProviderByIdAsync(_integrationTestFixture.ProviderId);

            //Assert
            Assert.NotEqual(documentUpdated, providerUpdatedViewModel.Data.Document);
        }

        [Fact(DisplayName = "Update invalid provider"), Priority(4)]
        [Trait("Categoria", "Fornecedor - Integração")]
        public async Task Update_UpdateInvalidProvider_MustNotUpdateAndReturnMessageNotification()
        {
            //Arrange
            var providerViewModel = new ProviderViewModel();

            //Act
            var response = await _integrationTestFixture.Client.PutAsJsonAsync(_requestUpdate + providerViewModel.Id, providerViewModel);
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<ProviderViewModel>>(response);

            Assert.False(responseDeserialized.Success);
        }

        [Fact(DisplayName = "Update valid provider don't save"), Priority(4)]
        [Trait("Categoria", "Fornecedor - Integração")]
        public async Task Update_UpdateValidProviderDontSave_MustNotUpdateAndReturnMessageNotification()
        {
            //Arrange
            var providerFixture = new ProviderTestFixture();
            var providerViewModel = providerFixture.GenerateValidProvider();

            //Act
            var response = await _integrationTestFixture.Client.PutAsJsonAsync(_requestUpdate + providerViewModel.Id, providerViewModel);
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<ProviderViewModel>>(response);

            Assert.False(responseDeserialized.Success);
        }

        [Fact(DisplayName = "Update valid provider with invalid address"), Priority(4)]
        [Trait("Categoria", "Fornecedor - Integração")]
        public async Task Update_UpdateValidProviderWithInvalidAddress_MustNotUpdateAndReturnMessageNotification()
        {
            //Arrange
            var providerViewModel = await GetProviderByIdAsync(_integrationTestFixture.ProviderId);
            providerViewModel.Data.Addresses = new List<AddressViewModel>{ new AddressViewModel () };

            //Act
            var response = await _integrationTestFixture.Client.PutAsJsonAsync(_requestUpdate + providerViewModel.Data.Id, providerViewModel);
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<ProviderViewModel>>(response);

            //Assert
            Assert.False(responseDeserialized.Success);
        }

        [Fact(DisplayName = "Update valid provider with invalid contact"), Priority(4)]
        [Trait("Categoria", "Fornecedor - Integração")]
        public async Task Update_UpdateValidProviderWithInvalidContact_MustNotUpdateAndReturnMessageNotification()
        {
            //Arrange
            var providerViewModel = await GetProviderByIdAsync(_integrationTestFixture.ProviderId);
            providerViewModel.Data.Contacts = new List<ContactViewModel> { new ContactViewModel() };

            //Act
            var response = await _integrationTestFixture.Client.PutAsJsonAsync(_requestUpdate + providerViewModel.Data.Id, providerViewModel);
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<ProviderViewModel>>(response);

            //Assert
            Assert.False(responseDeserialized.Success);
        }

        #endregion

        #region Function Remove
        [Fact(DisplayName = "Remove provider with success"), Priority(5)]
        [Trait("Categoria", "Fornecedor - Integração")]
        public async Task Remove_RemoveProvider_MustRemoveWithSuccess()
        {
            //Arrange anc act
            var response = await _integrationTestFixture.Client.DeleteAsync(_requestRemove + _integrationTestFixture.ProviderId);
            var providerRemoved = await GetProviderByIdAsync(_integrationTestFixture.ProviderId);

            //Assert
            response.EnsureSuccessStatusCode();
            Assert.Null(providerRemoved);
        }

        [Fact(DisplayName = "Remove provider with invalid id"), Priority(5)]
        [Trait("Categoria", "Fornecedor - Integração")]
        public async Task Remove_RemoveProviderWithIinvalidId_MustNotRemoveAndReturnMessageNotification()
        {
            //Arrange anc act
            var response = await _integrationTestFixture.Client.DeleteAsync(_requestRemove + Guid.Empty);
            var responseDeserialized =await TestTools.DeserializeResponseAsync<Deserialize<ProductViewModel>>(response);

            //Assert
            Assert.False(responseDeserialized.Success);
            Assert.Contains(ConstantMessages.ProviderNotFound, responseDeserialized.Errors);
        }
        #endregion

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
        private async Task<Deserialize<ProviderViewModel>> GetProviderByIdAsync(Guid id)
        {
            var response = await _integrationTestFixture.Client.GetAsync(_requestGetById + id);
            var responseDeserialized = await TestTools.DeserializeResponseAsync<Deserialize<ProviderViewModel>>(response);
            return responseDeserialized;
        }
        #endregion
    }
}
