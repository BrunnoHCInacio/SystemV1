using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Test.Fixture;
using SystemV1.Domain.Test.IntegrationTest;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(AddressCollection))]
    public class AddressCollection : ICollectionFixture<AddressTestFixture>
    { }

    public class AddressTestFixture : IDisposable
    {
        public void Dispose()
        {
        }

        public dynamic GenerateAddressExpected()
        {
            var stateFixture = new StateTestFixture();
            var faker = new Faker("pt_BR");
            return new
            {
                Id = Guid.NewGuid(),
                ZipCode = faker.Address.ZipCode(),
                Street = faker.Address.StreetAddress(),
                Number = faker.Random.Number(1, 9999).ToString(),
                Complement = faker.Address.SecondaryAddress(),
                District = faker.Address.Direction(),
                State = stateFixture.GenerateStateWithCountryValidExpected(),
                CityId = Guid.NewGuid(),
                CityName = faker.Address.City()
            };
        }

        public dynamic GenerateStateWithCountryValidExpected()
        {
            var stateFixture = new StateTestFixture();
            return stateFixture.GenerateStateWithCountryValidExpected();
        }

        public dynamic GenerateCountryExpected()
        {
            var countryFixture = new CountryTestFixture();
            return countryFixture.GenerateCountryExpected();
        }

        public List<Address> GenerateAddress(int quantity, bool registerEnable = true)
        {
            var address = new Faker<Address>("pt_BR")
                            .CustomInstantiator(f => new Address(Guid.NewGuid(),
                                                                f.Address.ZipCode(),
                                                                f.Address.StreetName(),
                                                                f.Random.Number(1, 99999).ToString(),
                                                                f.Address.SecondaryAddress(),
                                                                f.Address.Direction(),
                                                                Guid.NewGuid(),
                                                                Guid.NewGuid()));

            return address.Generate(quantity).ToList();
        }

        public Address GenerateValidAddress()
        {
            return GenerateAddress(1).FirstOrDefault();
        }

        public Address GenerateValidAddressNotActive()
        {
            return GenerateAddress(1, false).FirstOrDefault();
        }

        public Address GenerateInvalidAddress()
        {
            return new Address(new Guid(), null, null, null, null, null, Guid.Empty, Guid.Empty);
        }

        public List<AddressViewModel> GenerateValidAddressViewModel(int quantity, List<CityViewModel> citiesViewModel)
        {
            var city = new CityViewModel();
            var address = GenerateAddress(quantity);

            var addressesViewModel = new List<AddressViewModel>();

            if (citiesViewModel != null)
            {
                city = TestTools.GetRandomEntityInList<CityViewModel>(citiesViewModel);
            }

            addressesViewModel.AddRange(address.Select(a => new AddressViewModel
            {
                Id = a.Id,
                Street = a.Street,
                Number = a.Number,
                Complement = a.Complement,
                ZipCode = a.ZipCode,
                District = a.District,
                CityId = city.Id,
                CityName = city.Name
            }));

            return addressesViewModel;
        }

        public List<AddressViewModel> GenerateInvalidAddressViewModel()
        {
            var invalidAddress = GenerateInvalidAddress();
            return new List<AddressViewModel>() {
                new AddressViewModel
                {
                    Id = invalidAddress.Id,
                    Street = invalidAddress.Street,
                    Number = invalidAddress.Number,
                    Complement = invalidAddress.Complement,
                    ZipCode = invalidAddress.ZipCode,
                    District = invalidAddress.District,
                }
            };
        }

        public List<AddressViewModel> GenerateAddress(int qtyAddress,
                                                      bool isValidAddress,
                                                      AddressTestFixture addressFixture,
                                                      List<CityViewModel> citiesViewModel)
        {
            if (qtyAddress > 0)
            {
                if (isValidAddress)
                {
                    return addressFixture.GenerateValidAddressViewModel(qtyAddress, citiesViewModel);
                }
                else
                {
                    return addressFixture.GenerateInvalidAddressViewModel();
                }
            }
            return null;
        }
    }
}