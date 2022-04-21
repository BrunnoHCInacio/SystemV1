using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SystemV1.Application.Resources;

namespace SystemV1.Application.ViewModels
{
    public class CityViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = ConstantMessages.CityNameLength_PT)]
        [StringLength(100, ErrorMessage = ConstantMessages.CityNameLength_PT, MinimumLength = 2)]
        public string Name { get; set; }
        public Guid? StateId { get; set; }
        public string StateName { get; set; }
    }
}
