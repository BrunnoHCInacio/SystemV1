using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SystemV1.Application.Resources;

namespace SystemV1.Application.ViewModels
{
    public class AddressViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = ConstantMessages.StreetRequiredt_PT)]
        [StringLength(100, ErrorMessage = ConstantMessages.StreetLength100_PT, MinimumLength = 1)]
        public string Street { get; set; }

        public string Number { get; set; }
        public string Complement { get; set; }

        [Required(ErrorMessage = ConstantMessages.CountryNameRequired_PT)]
        public Guid? IdCountry { get; set; }

        public string CountryName { get; set; }

        [Required(ErrorMessage = ConstantMessages.CityNameRequired_PT)]
        [StringLength(50, ErrorMessage = ConstantMessages.CityNameLength_PT, MinimumLength = 2)]
        public string District { get; set; }

        [Required(ErrorMessage = ConstantMessages.StateRequired_PT)]
        public Guid? IdState { get; set; }

        public string StateName { get; set; }

        public string ZipCode { get; set; }
        public string City { get; set; }

        public Guid? IdCLient { get; set; }
        public Guid? IdProvider { get; set; }
    }
}