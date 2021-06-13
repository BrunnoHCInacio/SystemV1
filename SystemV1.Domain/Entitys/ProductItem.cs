using FluentValidation.Results;
using System;
using SystemV1.Domain.Validations;

namespace SystemV1.Domain.Entitys
{
    public class ProductItem : Entity
    {
        public ProductItem(Guid id,
                           string modelo,
                           decimal value,
                           string imageZip = null)
        {
            Id = id;
            Modelo = modelo;
            Value = value;
            ImageZip = imageZip;
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
            IsSold = true;
        }

        public void SetProductItemSell()
        {
            IsSold = false;
        }

        public void SetProductItemAvailable()
        {
            IsAvailable = true;
        }

        public void SetProductItemUnAvailable()
        {
            IsAvailable = false;
        }

        public ValidationResult ValidateProductItem()
        {
            return new ProductItemValidation().Validate(this);
        }
    }
}