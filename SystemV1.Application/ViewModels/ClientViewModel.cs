using System;
using System.ComponentModel.DataAnnotations;

namespace SystemV1.Application.ViewModels
{
    public class ClientViewModel
    {
        public ClientViewModel()
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