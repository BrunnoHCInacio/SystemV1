using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Interfaces.Mapper
{
    public interface IMapperProductItem
    {
        ProductItemViewModel EntityToViewModel(ProductItem product);

        ProductItem ViewModelToEntity(ProductItemViewModel productViewModel);

        IEnumerable<ProductItemViewModel> ListEntityToViewModel(IEnumerable<ProductItem> products);
    }
}