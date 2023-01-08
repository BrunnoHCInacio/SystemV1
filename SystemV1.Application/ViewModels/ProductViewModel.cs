using System;
using System.Collections.Generic;

namespace SystemV1.Application.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public Guid providerId { get; set; }

        public IEnumerable<ProductItemViewModel> ProductItems { get; set; }
    }
}