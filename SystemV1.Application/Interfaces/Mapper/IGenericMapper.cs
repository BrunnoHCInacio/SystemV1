using System.Collections.Generic;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Interfaces.Mapper
{
    public interface IGenericMapper<TEntity, TViewModel> where TEntity : Entity
    {
        TViewModel EntityToViewModel(TEntity entity);

        TEntity ViewModelToEntity(TViewModel viewModel);

        List<TViewModel> ListEntityToViewModel(List<TEntity> entities);

        List<TEntity> ListViewModelToEntity(List<TViewModel> viewModels);
    }
}