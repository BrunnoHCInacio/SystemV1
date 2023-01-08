using System;
using System.ComponentModel.DataAnnotations;
using SystemV1.Application.Resources;
using SystemV1.Domain.Enums;

namespace SystemV1.Application.ViewModels
{
    public class ContactViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = ConstantMessages.TypeContactRequired_PT)]
        public EnumTypeContact TypeContact { get; set; }

        public string Ddd { get; set; }
        public string Ddi { get; set; }
        public string CellPhoneNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        public Guid peopleId { get; set; }
    }
}