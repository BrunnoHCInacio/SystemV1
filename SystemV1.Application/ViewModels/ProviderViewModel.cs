using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using SystemV1.Application.Resources;

namespace SystemV1.Application.ViewModels
{
    public class ProviderViewModel
    {
        public ProviderViewModel()
        {
            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
            }
        }

        [Key]
        public Guid Id { get; set; }

        public Guid PeopleId { get; set; }
    }
}