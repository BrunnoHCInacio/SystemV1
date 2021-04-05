using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SystemV1.Application.Resources;

namespace SystemV1.Application.ViewModels
{
    public class StateViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = ConstantMessages.StateRequired_PT)]
        [StringLength(100, ErrorMessage = ConstantMessages.StateNameLenght_Pt, MinimumLength = 2)]
        public string Name { get; set; }
    }
}