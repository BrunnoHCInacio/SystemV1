using System;
using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = ConstantMessages.DistrictRequired_PT)]
        [StringLength(100, ErrorMessage = ConstantMessages.DistrictLength_PT, MinimumLength = 2)]
        public string District { get; set; }

        public string ZipCode { get; set; }

        [Required(ErrorMessage = ConstantMessages.CityRequired_PT)]
        public Guid CityId { get; set; }

        public string CityName { get; set; }
    }
}