using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.ViewModels;
using SystemV1.Domain.Entitys;

namespace SystemV1.Application.Mappers
{
    public class MapperProduct : IMapperProduct
    {
        private readonly IMapperProductItem _mapperProductItem;

        public MapperProduct(IMapperProductItem mapperProductItem)
        {
            _mapperProductItem = mapperProductItem;
        }

        public ProductViewModel EntityToViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                ProviderName = product.Provider?.Name ?? "",
                IdProvider = product.Provider?.Id
            };
        }

        public IEnumerable<ProductViewModel> ListEntityToViewModel(IEnumerable<Product> products)
        {
            return products.Select(c => EntityToViewModel(c));
        }

        public Product ViewModelToEntity(ProductViewModel productViewModel)
        {
            var product = new Product(productViewModel.Id, productViewModel.Name);

            product.SetProvider(new Provider(productViewModel.IdProvider.GetValueOrDefault(), null, null));
            product.AddProductItems(productViewModel.ProductItems.Select(pi => _mapperProductItem.ViewModelToEntity(pi)).ToList());

            return product;
        }
    }
}