using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using SystemV1.Application.Resources;

namespace SystemV1.Application.ViewModels
{
    public class CountryViewModel
    {
        [Key]
        [JsonProperty("id")]
        public Guid? Id { get; set; }

        [Required(ErrorMessage = ConstantMessages.CountryNameRequired_PT)]
        [StringLength(100, ErrorMessage = ConstantMessages.CountryNameLengh_PT, MinimumLength = 2)]
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}