using API2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Application.ViewModels.Identity;
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
                await AddAsync(viewModel, urlRequest, sucessCase);
            }
        }

        protected async Task<HttpResponseMessage> AddAsync<TViewModel>(TViewModel viewModel,
                                                  string urlRequest,
                                                  bool sucessCase = true,
                                                  bool useAuthorize = true)
        {
            //Arrange
            if (useAuthorize)
            {
                await GetAuthorizationAsync();
            }

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

        protected async Task<TListResult> GetAllAsync<TListResult>(string urlRequest,
                                                                   bool sucessCase = true,
                                                                   bool useAuthorize = true)
        {
            if (useAuthorize)
            {
                await GetAuthorizationAsync();
            }

            var response = await _integrationTestFixture.Client.GetAsync(urlRequest);

            if (sucessCase)
            {
                response.EnsureSuccessStatusCode();
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();
            return DeserializeJson<TListResult>(jsonResponse);
        }

        protected async Task<TViewModel> GetByIdAsync<TViewModel>(Guid id,
                                                                  string urlRquest,
                                                                  bool useAutorize = true)
        {
            if (useAutorize)
            {
                await GetAuthorizationAsync();
            }

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
                                                                           bool succesCase = true,
                                                                           bool useAuthorize = true)
        {
            if (useAuthorize)
            {
                await GetAuthorizationAsync();
            }

            var responseUpdate = await _integrationTestFixture
                                            .Client
                                            .PutAsJsonAsync($"{urlRequest}{id}", viewModel);

            if (succesCase)
            {
                responseUpdate.EnsureSuccessStatusCode();
            }

            var jsonCountryUpdated = await responseUpdate.Content.ReadAsStringAsync();
            return DeserializeJson<Deserialize<TViewModel>>(jsonCountryUpdated);
        }

        #endregion Update

        #region Remove

        protected async Task<HttpResponseMessage> RemoveAsync(Guid id,
                                                              string urlRequest,
                                                              bool useAuthorize = true)
        {
            //Arrange
            if (useAuthorize)
            {
                await GetAuthorizationAsync();
            }

            //Act
            return await _integrationTestFixture.Client.DeleteAsync($"{urlRequest}{id}");
        }

        #endregion Remove

        protected TObject DeserializeJson<TObject>(string json)
        {
            return JsonConvert.DeserializeObject<TObject>(json);
        }

        protected async Task RegisterUserAsync(RegisterUserViewModel registerUser)
        {
            if (registerUser == null)
            {
                registerUser = new RegisterUserViewModel
                {
                    Email = "system@system.com",
                    Password = "Test@1234",
                    ConfirmPassword = "Test@1234"
                };
            }

            await _integrationTestFixture.Client.PostAsJsonAsync("api/auth/RegisterUser", registerUser);

            _integrationTestFixture.Email = registerUser.Email;
            _integrationTestFixture.Password = registerUser.Password;
        }

        private async Task GetAuthorizationAsync()
        {
            if (string.IsNullOrEmpty(_integrationTestFixture.Email)
                && string.IsNullOrEmpty(_integrationTestFixture.Password))
            {
                await RegisterUserAsync(null);
            }

            var user = new LoginViewModel
            {
                Email = _integrationTestFixture.Email,
                Password = _integrationTestFixture.Password
            };

            var response = await _integrationTestFixture.Client.PostAsJsonAsync("api/auth/Login", user);
            var responseJson = await response.Content.ReadAsStringAsync();

            var responseData = DeserializeJson<Deserialize<string>>(responseJson);

            _integrationTestFixture.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", responseData.Data);
        }
    }
}