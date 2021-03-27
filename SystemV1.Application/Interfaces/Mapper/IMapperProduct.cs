using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Interfaces.Mapper
{
    public interface IMapperProduct
    {
        ProductViewModel EntityToViewModel(Product product);

        Product ViewModelToEntity(ProductViewModel productViewModel);

        IEnumerable<ProductViewModel> ListEntityToViewModel(IEnumerable<Product> products);
    }
}