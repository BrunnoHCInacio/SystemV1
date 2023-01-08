using SystemV1.Domain.Test.Fixture;
using Xunit;

namespace SystemV1.Domain.Test.DomainTests
{
    [Collection(nameof(ClientCollection))]
    public class ClientDomainTest
    {
        private readonly ClientTestFixture _clientTestFixture;

        public ClientDomainTest(ClientTestFixture clientTestFixture)
        {
            _clientTestFixture = clientTestFixture;
        }

        [Fact(DisplayName = "Validate valid client")]
        [Trait("UnitTests - Entity", "Client")]
        public void Client_ValidateClient_ShouldBeValid()
        {
            //Arrange
            //var client = _clientTestFixture.GenerateValidClient();

            ////Act
            //var result = client.ValidateClient();

            ////Assert
            //Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validate invalid client")]
        [Trait("UnitTests - Entity", "Client")]
        public void Client_ValidateClient_ShouldBeInvalid()
        {
            //Arrange
            //var client = _clientTestFixture.GenerateInvalidClient();

            ////Act
            //var result = client.ValidateClient();

            ////Assert
            //Assert.False(result.IsValid);
            //Assert.True(result.Errors.Any());
            //Assert.Equal(2, result.Errors.Count);

            //Assert.Contains(ClientValidation.NameRequired, result.Errors.Select(e => e.ErrorMessage));
            //Assert.Contains(ClientValidation.DocumentRequired, result.Errors.Select(e => e.ErrorMessage));
        }
    }
}