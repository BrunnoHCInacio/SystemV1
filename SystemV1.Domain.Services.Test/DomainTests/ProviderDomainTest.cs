using FluentAssertions;
using System.Linq;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Test.Fixture;
using SystemV1.Domain.Validations;
using Xunit;

namespace SystemV1.Domain.Test.DomainTests
{
    [Collection(nameof(ProviderCollection))]
    public class ProviderDomainTest
    {
        private readonly ProviderTestFixture _providerTestFixture;

        public ProviderDomainTest(ProviderTestFixture providerTestFixture)
        {
            _providerTestFixture = providerTestFixture;
        }

        [Fact(DisplayName = "Validate set as correct properties for provider")]
        [Trait("Categoria","Fornecedor - Cadastro")]
        public void ProviderDomain_SetAndVerifyProperties_ShouldSetAsCorrectValueOnProperties()
        {
            //Arrange
            var providerExpected = _providerTestFixture.GenerateProviderExpected();

            //Act
            var provider = new Provider(providerExpected.Id,
                                        providerExpected.Name,
                                        providerExpected.Document);

            //Assert
            provider.Should().NotBeNull();
            provider.Id.Should().Be(providerExpected.Id);
            provider.Name.Should().Be(providerExpected.Name);
            provider.Document.Should().Be(providerExpected.Document);
        }

        [Fact(DisplayName = "Validate set as correct properties for provider with addresses and contacts")]
        [Trait("Categoria", "Fornecedor - Cadastro")]
        public void ProviderDomain_SetProprietiesForProviderWithContactsAndAddress_ShouldSetAsCorrectValueOnProperties()
        {
            //Arrange
            var providerExpected = _providerTestFixture.GenerateProviderExpected();
            var addressExpected = new AddressTestFixture().GenerateAddressExpected();

            //Act
            var provider = new Provider(providerExpected.Id,
                                        providerExpected.Name,
                                        providerExpected.Document);
            var address = new Address(addressExpected.Id,
                                      addressExpected.ZipCode,
                                      addressExpected.Street,
                                      addressExpected.Number,
                                      addressExpected.Complement,
                                      addressExpected.District);
            address.SetCity(addressExpected.CityId);
            provider.AddAddress(address);

            //Assert
            provider.Should().NotBeNull();
            provider.Id.Should().Be(providerExpected.Id);
            provider.Name.Should().Be(providerExpected.Name);
            provider.Document.Should().Be(providerExpected.Document);

            foreach (var itemAddress in provider.Addresses)
            {
                itemAddress.Id.Should().Be(addressExpected.Id);
                itemAddress.ZipCode.Should().Be(addressExpected.ZipCode);
                itemAddress.Street.Should().Be(addressExpected.Street);
                itemAddress.Number.Should().Be(addressExpected.Number);
                itemAddress.Complement.Should().Be(addressExpected.Complement);
                itemAddress.District.Should().Be(addressExpected.District);
                itemAddress.CityId.Should().Be(addressExpected.CityId);

            }
        }

        [Fact(DisplayName ="Verify validation of provider")]
        [Trait("Categoria","Fornecedor - Cadastro")]
        public void ProviderDmain_ValidateDomain_ShouldBeValid()
        {
            //Arrange
            var providerExpected = _providerTestFixture.GenerateProviderExpected();

            //act
            var provider = new Provider(providerExpected.Id,
                                        providerExpected.Name, 
                                        providerExpected.Document);
            var result = provider.ValidateProvider();

            //Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact(DisplayName ="Verify validation for invalid domain")]
        [Trait("Categoria","Fornecedor - Cadastro")]
        public void ProviderDomain_InvalidDomain_ShouldBeInvalidAndReturnMessages()
        {
            //Arrange
            var providerExpected = _providerTestFixture.GenerateInvalidProvider();

            //Act
            var provider = new Provider(providerExpected.Id,
                                       providerExpected.Name,
                                       providerExpected.Document);
            var result = provider.ValidateProvider();

            //Assert
            result.IsValid.Should().BeFalse();
            Assert.Contains(ProviderValidation.NameRequired, result.Errors.Select(e => e.ErrorMessage));
        }
    }
}
