using System;
using System.Collections.Generic;
using System.Text;

namespace SystemV1.Application.ViewModels
{
    public class ProductViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public Guid? IdProvider { get; set; }

        public IEnumerable<ProductItemViewModel> ProductItems { get; set; }
    }
}