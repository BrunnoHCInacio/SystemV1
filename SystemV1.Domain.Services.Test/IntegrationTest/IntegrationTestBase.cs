using API2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Test.IntegrationTest.Config;

namespace SystemV1.Domain.Test.IntegrationTest
{
    public abstract class IntegrationTestBase
    {
        private readonly IntegrationTestFixture<StartupApiTests> _integrationTestFixture;

        protected IntegrationTestBase(IntegrationTestFixture<StartupApiTests> integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
        }

        #region Add

        protected async Task AddListAsync<TViewModel>(List<TViewModel> listViewModel,
                                                      string urlRequest,
                                                      bool sucessCase = true)
        {
            foreach (var viewModel in listViewModel)
            {
                await AddAsync<TViewModel>(viewModel, urlRequest, sucessCase);
            }
        }

        protected async Task<HttpResponseMessage> AddAsync<TViewModel>(TViewModel viewModel,
                                                  string urlRequest,
                                                  bool sucessCase = true)
        {
            //Act
            var response = await _integrationTestFixture.Client.PostAsJsonAsync(urlRequest, viewModel);

            if (sucessCase)
            {
                //Assert
                response.EnsureSuccessStatusCode();
            }

            return response;
        }

        #endregion Add

        #region Get

        protected async Task<TListResult> GetAllAsync<TListResult>(string urlRequest, bool sucessCase = true)
        {
            var response = await _integrationTestFixture.Client.GetAsync(urlRequest);

            if (sucessCase)
            {
                response.EnsureSuccessStatusCode();
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            return DeserializeJson<TListResult>(jsonResponse);
        }

        protected async Task<TViewModel> GetByIdAsync<TViewModel>(Guid id, string urlRquest)
        {
            var response = await _integrationTestFixture.Client.GetAsync($"{urlRquest}{id}");
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TViewModel>(jsonResponse);
        }

        #endregion Get

        #region Update

        protected async Task<Deserialize<TViewModel>> UpdateAsync<TViewModel>(TViewModel viewModel,
                                                                           Guid id,
                                                                           string urlRequest,
                                                                           bool verifyStateCode = false)
        {
            var responseUpdate = await _integrationTestFixture
                                            .Client
                                            .PutAsJsonAsync($"{urlRequest}{id}", viewModel);

            if (verifyStateCode)
            {
                responseUpdate.EnsureSuccessStatusCode();
            }

            var jsonCountryUpdated = await responseUpdate.Content.ReadAsStringAsync();
            return DeserializeJson<Deserialize<TViewModel>>(jsonCountryUpdated);
        }

        #endregion Update

        #region Remove

        protected async Task<HttpResponseMessage> RemoveAsync(Guid id,
                                                              string urlRequest)
        {
            //Arrange and Act
            return await _integrationTestFixture.Client.DeleteAsync($"{urlRequest}{id}");
        }

        #endregion Remove

        protected TObject DeserializeJson<TObject>(string json)
        {
            return JsonConvert.DeserializeObject<TObject>(json);
        }
    }
}