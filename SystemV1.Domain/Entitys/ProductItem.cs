using System;

namespace SystemV1.Domain.Entitys
{
    public class ProductItem : Entity
    {
        public ProductItem()
        {
        }

        public ProductItem(Guid id,
                           string modelo,
                           decimal value,
                           Product product,
                           string imageZip = null)
        {
            Id = id;
            Modelo = modelo;
            Value = value;
            ImageZip = imageZip;
            Product = product;
            ProductId = product?.Id ?? Guid.Empty;
        }

        public Product Product { get; private set; }
        public Guid ProductId { get; set; }

        public string Modelo { get; private set; }
        public decimal Value { get; private set; }
        public bool IsSold { get; private set; }
        public bool IsAvailable { get; private set; }
        public string ImageZip { get; private set; }

        public void SetProductItemSold()
        {
            SetProductItemUnAvailable();
            IsSold = true;
        }

        public void SetProductItemAvailable()
        {
            IsSold = false;
            IsAvailable = true;
        }

        private void SetProductItemUnAvailable()
        {
            IsAvailable = false;
        }

        public void SetProduct(Product product)
        {
            Product = product;
            ProductId = product.Id;
        }
    }
}