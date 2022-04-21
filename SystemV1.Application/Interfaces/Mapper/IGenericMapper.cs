using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Interfaces.Mapper
{
    public interface IGenericMapper<TEntity, TViewModel> where TEntity : Entity
    {
        TViewModel EntityToViewModel(TEntity entity);

        TEntity ViewModelToEntity(TViewModel viewModel);

        IEnumerable<TViewModel> ListEntityToViewModel(IEnumerable<TEntity> entities);

        List<TEntity> ListViewModelToEntity(IEnumerable<TViewModel> viewModels);
    }
}
