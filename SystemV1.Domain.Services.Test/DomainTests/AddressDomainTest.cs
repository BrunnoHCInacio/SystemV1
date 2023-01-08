using System;
using System.Linq;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Test.Fixture;
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
        [Trait("UnitTests - Entity", "Address")]
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
                                      addressExpected.District,
                                      addressExpected.CityId,
                                      Guid.NewGuid());

            //Assert
            Assert.Equal(address.Id, addressExpected.Id);
            Assert.Equal(address.Street, addressExpected.Street);
            Assert.Equal(address.ZipCode, addressExpected.ZipCode);
            Assert.Equal(address.Number, addressExpected.Number);
            Assert.Equal(address.Complement, addressExpected.Complement);
            Assert.Equal(address.District, addressExpected.District);
            Assert.Equal(address.CityId, addressExpected.CityId);
        }
    }
}