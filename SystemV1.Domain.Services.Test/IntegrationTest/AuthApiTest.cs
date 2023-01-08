using API2;
using System.Net;
using System.Threading.Tasks;
using SystemV1.Application.Resources;
using SystemV1.Application.ViewModels.Identity;
using SystemV1.Domain.Test.Fixture;
using SystemV1.Domain.Test.IntegrationTest.Config;
using Xunit;
using Xunit.Priority;

namespace SystemV1.Domain.Test.IntegrationTest
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    [Collection(nameof(IntegrationApiTestFixtureCollection))]
    public class AuthApiTest : IntegrationTestBase
    {
        private readonly IntegrationTestFixture<StartupApiTests> _integrationTestFixture;
        private readonly AuthUserFixture _authUserFixture;

        private string _requestRegisterUser = "api/auth/RegisterUser";
        private string _requestLoginUser = "api/auth/Login";

        public AuthApiTest(IntegrationTestFixture<StartupApiTests> integrationTestFixture) : base(integrationTestFixture)
        {
            _integrationTestFixture = integrationTestFixture;
            _authUserFixture = new AuthUserFixture();
        }

        #region Create user

        [Fact(DisplayName = "register new user with success"), Priority(1)]
        [Trait("UnitTests - Integration", "Auth")]
        public async Task RegisterUser_RegisterValidUser_ShouldRegisterWithSuccess()
        {
            //Arrange
            var user = _authUserFixture.GenerateValidRegisterUser();

            //act and assert
            await AddAsync(user, _requestRegisterUser);

            _integrationTestFixture.Email = user.Email;
            _integrationTestFixture.Password = user.Password;
        }

        [Fact(DisplayName = "register new user with invalid email"), Priority(1)]
        [Trait("UnitTests - Integration", "Auth")]
        public async Task RegisterUser_RegisterUserWithInvalidEmail_ShouldNotRegisterAndReturnFalied()
        {
            //Arrange
            var user = _authUserFixture.GenerateValidRegisterUser();
            user.Email = "admin_admin";

            //act
            var result = await AddAsync(user, _requestRegisterUser, false);
            var json = DeserializeJson<Deserialize<RegisterUserViewModel>>(await result.Content.ReadAsStringAsync());

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.False(json.Success);
            Assert.Contains("O campo Email está em formato inválido.", json.Errors);
        }

        [Fact(DisplayName = "register new user with invalid password"), Priority(1)]
        [Trait("UnitTests - Integration", "Auth")]
        public async Task RegisterUser_RegisterUserWithInvalidPassWord_ShouldNotRegisterAndReturnFailed()
        {
            //Arrange

            var invalidPassword = "1234567";
            var user = _authUserFixture.GenerateValidRegisterUser(password: invalidPassword);

            //act
            var result = await AddAsync(user, _requestRegisterUser, false);
            var json = DeserializeJson<Deserialize<RegisterUserViewModel>>(await result.Content.ReadAsStringAsync());

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.False(json.Success);
            Assert.Contains("O campo Password precisa ter entre 8 e 100 caracter.", json.Errors);
        }

        [Fact(DisplayName = "register new user with password does not match"), Priority(1)]
        [Trait("UnitTests - Integration", "Auth")]
        public async Task RegisterUser_RegisterUserWithPasswordDoesNotMatch_ShouldNotRegisterAndReturnFailed()
        {
            //Arrange
            var invalidPassword = "1234567";
            var user = _authUserFixture.GenerateValidRegisterUser(confirmPassword: invalidPassword);

            //act
            var result = await AddAsync(user, _requestRegisterUser, false);
            var json = DeserializeJson<Deserialize<RegisterUserViewModel>>(await result.Content.ReadAsStringAsync());

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.False(json.Success);
            Assert.Contains("As senhas não conferem.", json.Errors);
        }

        [Fact(DisplayName = "register new user with password does not match"), Priority(1)]
        [Trait("UnitTests - Integration", "Auth")]
        public async Task RegisterUser_RegisterNewInvalidUser_ShouldNotRegisterAndReturnFailed()
        {
            //Arrange
            var user = new RegisterUserViewModel();

            //act
            var result = await AddAsync(user, _requestRegisterUser, false);
            var json = DeserializeJson<Deserialize<RegisterUserViewModel>>(await result.Content.ReadAsStringAsync());

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.False(json.Success);
        }

        #endregion Create user

        #region Login user

        [Fact(DisplayName = "login user with valid credentials"), Priority(2)]
        [Trait("UnitTests - Integration", "Auth")]
        public async Task RegisterUser_LoginWithValidUser_ShouldLogonWithSuccess()
        {
            var user = new LoginViewModel
            {
                Email = _integrationTestFixture.Email,
                Password = _integrationTestFixture.Password
            };

            await AddAsync(user, _requestLoginUser);
        }

        #endregion Login user
    }
}