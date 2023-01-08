using System.Collections.Generic;
using System.Linq;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Mappers
{
    public class MapperProductItem : IMapperProductItem
    {
        public ProductItemViewModel EntityToViewModel(ProductItem product)
        {
            return new ProductItemViewModel
            {
                Id = product.Id,
                Modelo = product.Modelo,
                Value = product.Value,
                IsAvailable = product.IsAvailable,
                IsSold = product.IsSold,
                IdProduct = product.Product?.Id ?? null,
            };
        }

        public IEnumerable<ProductItemViewModel> ListEntityToViewModel(IEnumerable<ProductItem> products)
        {
            return products.Select(p => EntityToViewModel(p));
        }

        public ProductItem ViewModelToEntity(ProductItemViewModel productViewModel)
        {
            //var productItem = new ProductItem(productViewModel.Id, productViewModel.Modelo, productViewModel.Value);

            //productItem.SetProduct(new Product(productViewModel.IdProduct.GetValueOrDefault(), null));
            //return productItem;
            return null;
        }
    }
}