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

        [Required(ErrorMessage = ConstantMessages.CountryRequired_PT)]
        public Guid? IdCountry { get; set; }

        public string CountryName { get; set; }

        [Required(ErrorMessage = ConstantMessages.DistrictRequired_PT)]
        [StringLength(50, ErrorMessage = ConstantMessages.DistrictLength_PT, MinimumLength = 2)]
        public string District { get; set; }

        [Required(ErrorMessage = ConstantMessages.StateRequired_PT)]
        public Guid? IdState { get; set; }

        public string StateName { get; set; }

        [Required(ErrorMessage = ConstantMessages.ZipCodeRequiredt_PT)]
        [StringLength(8, ErrorMessage = ConstantMessages.ZipCodeLength_PT, MinimumLength = 7)]
        public int ZipCode { get; set; }

        public Guid? IdCLient { get; set; }
        public Guid? IdProvider { get; set; }
    }
}