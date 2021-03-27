using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SystemV1.Application.Resources;

namespace SystemV1.Application.ViewModels
{
    public class ContactViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = ConstantMessages.TypeContactRequired_PT)]
        public string TypeContact { get; set; }

        public string Ddd { get; set; }
        public string Ddi { get; set; }
        public string CellPhoneNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public Guid? IdProvider { get; set; }
        public Guid? IdClient { get; set; }
    }
}