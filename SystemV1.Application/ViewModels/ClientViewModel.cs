using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SystemV1.Application.Resources;

namespace SystemV1.Application.ViewModels
{
    public class ClientViewModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = ConstantMessages.PeopleNameRequired_PT)]
        [StringLength(100, ErrorMessage = ConstantMessages.PeopleNameLength100_PT, MinimumLength = 0)]
        public string Name { get; set; }

        [Required(ErrorMessage = ConstantMessages.ClientDocumentRequired_PT)]
        [StringLength(18, ErrorMessage = ConstantMessages.ClientDocumentLength_PT, MinimumLength = 11)]
        public string Document { get; set; }

        public IEnumerable<AddressViewModel> Addresses { get; set; }
        public IEnumerable<ContactViewModel> Contacts { get; set; }
    }
}