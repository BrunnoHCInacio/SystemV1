using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SystemV1.Application.Resources;

namespace SystemV1.Application.ViewModels
{
    public class ProductItemViewModel
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? IdProduct { get; set; }

        [Required(ErrorMessage = ConstantMessages.ProductItemModelRequired_PT)]
        [StringLength(150, ErrorMessage = ConstantMessages.ProductItemModelLength_PT, MinimumLength = 2)]
        public string Modelo { get; set; }

        [Required(ErrorMessage = ConstantMessages.ProductItemValueRequired_PT)]
        public decimal Value { get; set; }

        public bool IsSold { get; set; }
        public bool IsAvailable { get; set; }
        public string ImageEncoded { get; set; }
    }
}