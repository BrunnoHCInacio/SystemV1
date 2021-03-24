using System.Collections.Generic;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Interfaces.Mapper
{
    public interface IMapperProvider
    {
        ProviderViewModel EntityToViewModel(Provider provider);

        Provider ViewModelToEntity(ProviderViewModel providerViewModel);

        IEnumerable<ProviderViewModel> ListEntityToViewModel(IEnumerable<Provider> providers);
    }
}