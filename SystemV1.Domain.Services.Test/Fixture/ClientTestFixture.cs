using Bogus;
using System;
using System.Collections.Generic;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;
using Xunit;

namespace SystemV1.Domain.Test.Fixture
{
    [CollectionDefinition(nameof(ClientCollection))]
    public class ClientCollection : ICollectionFixture<ClientTestFixture>
    {
    }

    public class ClientTestFixture : IDisposable
    {
        public dynamic GenerateClientExpected()
        {
            return new
            {
                Id = Guid.NewGuid(),
                PeopleId = Guid.NewGuid()
            };
        }

        public List<Client> GenerateClient(int quantity, Guid peopleId)

        {
            var client = new Faker<Client>("pt_BR")
                            .CustomInstantiator(f => new Client(Guid.NewGuid(), peopleId));

            return client.Generate(quantity);
        }

        public List<ClientViewModel> GenerateClientViewModel(int qtyClients, Guid peopleId)
        {
            var addressFixture = new AddressTestFixture();
            var contactsFixture = new ContactTestFixture();

            var client = new Faker<ClientViewModel>("pt_BR")
                                .FinishWith(
                                (f, c) =>
                                {
                                    c.PeopleId = peopleId;
                                });

            return client.Generate(qtyClients);
        }

        public ClientViewModel GenerateInvalidClientViewModel()
        {
            return new ClientViewModel();
        }

        public void Dispose()
        {
        }
    }
}