using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceContact : Service<Contact>, IServiceContact
    {
        private readonly IRepositoryContact _repositoryContact;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceContact(IRepositoryContact repositoryContact, IUnitOfWork unitOfWork) : base(repositoryContact, unitOfWork)
        {
            _repositoryContact = repositoryContact;
            _unitOfWork = unitOfWork;
        }

        public void Remove(Contact peopleContact)
        {
            peopleContact.IsActive = false;
        }

        public void RemoveUow(Contact peopleContact)
        {
            throw new NotImplementedException();
        }
    }
}