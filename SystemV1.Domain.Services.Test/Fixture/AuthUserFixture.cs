using Bogus;
using System;
using SystemV1.Application.ViewModels.Identity;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(AythUserCollection))]
    public class AythUserCollection : ICollectionFixture<AuthUserFixture>
    { }

    public class AuthUserFixture : IDisposable
    {
        private readonly string _defaultPassword = "Brunno@1234";

        public RegisterUserViewModel GenerateValidRegisterUser(string email = "",
                                                               bool useDefaultPassword = false,
                                                               string password = "",
                                                               string confirmPassword = "")
        {
            return new Faker<RegisterUserViewModel>("pt_BR").FinishWith(
                (f, r) =>
                {
                    r.Email = f.Internet.Email();
                    r.Password = _defaultPassword;
                    r.ConfirmPassword = r.Password;

                    if (!string.IsNullOrEmpty(email.Trim()))
                    {
                        r.Email = email;
                    }

                    if (useDefaultPassword)
                    {
                        r.Password = _defaultPassword;
                        r.ConfirmPassword = _defaultPassword;
                    }
                    if (!string.IsNullOrEmpty(password.Trim()))
                    {
                        r.Password = password;
                    }
                    if (!string.IsNullOrEmpty(confirmPassword.Trim()))
                    {
                        r.ConfirmPassword = confirmPassword;
                    }
                }).Generate();
        }

        public void Dispose()
        {
        }
    }
}