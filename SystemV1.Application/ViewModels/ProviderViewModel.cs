using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SystemV1.Application.Resources;

namespace SystemV1.Application.ViewModels
{
    public class ProviderViewModel

    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = ConstantMessages.PeopleNameRequired_PT)]
        [StringLength(100, ErrorMessage = ConstantMessages.PeopleNameLength100_PT, MinimumLength = 0)]
        public string Name { get; set; }

        [Required(ErrorMessage = ConstantMessages.ProviderDocumentRequired_PT)]
        [StringLength(14, ErrorMessage = ConstantMessages.ProviderDocumentLength_PT, MinimumLength = 14)]
        public string Document { get; set; }

        public IEnumerable<AddressViewModel> Addresses { get; set; }
        public IEnumerable<ContactViewModel> Contacts { get; set; }
    }
}