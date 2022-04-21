using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Test.Fixture;
using SystemV1.Domain.Test.Fixture;
using SystemV1.Domain.Validations;
using Xunit;

namespace SystemV1.Domain.Test
{
    [Collection(nameof(AddressCollection))]
    public class AddressDomainTest
    {
        public readonly AddressTestFixture _addressTestFixture;

        public AddressDomainTest(AddressTestFixture addressTestFixture)
        {
            _addressTestFixture = addressTestFixture;
        }

        [Fact(DisplayName = "Validate set as correct properties")]
        [Trait("Categoria", "Cadastro - Endereço")]
        public void Address_VerifyDomain_ValidatePropertiesDomain()
        {
            //Act & Arrange
            var addressExpected = _addressTestFixture.GenerateAddressExpected();

            //state.Country = country;
            var address = new Address(addressExpected.Id,
                                      addressExpected.ZipCode,
                                      addressExpected.Street,
                                      addressExpected.Number,
                                      addressExpected.Complement,
                                      addressExpected.District);
            address.SetCity(addressExpected.CityId);
            
            //Assert
            Assert.Equal(address.Id, addressExpected.Id);
            Assert.Equal(address.Street, addressExpected.Street);
            Assert.Equal(address.ZipCode, addressExpected.ZipCode);
            Assert.Equal(address.Number, addressExpected.Number);
            Assert.Equal(address.Complement, addressExpected.Complement);
            Assert.Equal(address.District, addressExpected.District);
            Assert.Equal(address.CityId, addressExpected.CityId);
        }

        [Fact(DisplayName = "Validate valid address")]
        [Trait("Categoria", "Cadastro - Endereço")]
        public void Address_ValidateAddress_ShouldBeValid()
        {
            //Arrange
            var address = _addressTestFixture.GenerateValidAddress();

            //Act
            var result = address.ValidateAddress();

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Validate a disable valid address")]
        [Trait("Categoria", "Cadastro - Endereço")]
        public void Address_ValidateAddress_ShouldFailed()
        {
            //Arrange 
            var address = _addressTestFixture.GenerateValidAddressNotActive();

            //Act
            var result = address.ValidateAddress();

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any());
            Assert.Single(result.Errors);
            Assert.Contains(AddressValidation.AddressNotActive, result.Errors.Select(e => e.ErrorMessage));
        }

        [Fact(DisplayName = "Validate invalid address")]
        [Trait("Categoria", "Cadastro - Endereço")]
        public void Address_ValidateAddress_ShouldBeInvalid()
        {
            //Arrange
            var address = _addressTestFixture.GenerateInvalidAddress();

            //Act
            var result = address.ValidateAddress();

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any());
            Assert.Equal(2, result.Errors.Count);

            Assert.Contains(AddressValidation.StreetRequired, result.Errors.Select(e => e.ErrorMessage));
            Assert.Contains(AddressValidation.StreetRequired, result.Errors.Select(e => e.ErrorMessage));
        }
    }
}