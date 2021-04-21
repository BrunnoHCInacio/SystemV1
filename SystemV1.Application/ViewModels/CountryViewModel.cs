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
        public Guid Id { get; set; }

        [Required(ErrorMessage = ConstantMessages.CountryRequired_PT)]
        [StringLength(100, ErrorMessage = ConstantMessages.CountryLengh_PT, MinimumLength = 2)]
        public string Name { get; set; }

        public IEnumerable<StateViewModel> States { get; set; }
    }
}