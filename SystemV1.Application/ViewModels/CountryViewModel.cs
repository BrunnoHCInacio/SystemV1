using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SystemV1.Application.Resources;

namespace SystemV1.Application.ViewModels
{
    public class CountryViewModel
    {
        public CountryViewModel()
        {
            States = new List<StateViewModel>();
        }

        [Key]
        [JsonProperty("id")]
        public Guid? Id { get; set; }

        [Required(ErrorMessage = ConstantMessages.CountryNameRequired_PT)]
        [StringLength(100, ErrorMessage = ConstantMessages.CountryNameLengh_PT, MinimumLength = 2)]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("states")]
        public IEnumerable<StateViewModel> States { get; set; }
    }
}