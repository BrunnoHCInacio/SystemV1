using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceAddress : Service<Address>, IServiceAddress
    {
        private readonly IRepositoryAddress _repositoryAddress;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceAddress(IRepositoryAddress repositoryAddress,
                                    IUnitOfWork unitOfWork)
            : base(repositoryAddress, unitOfWork)
        {
            _repositoryAddress = repositoryAddress;
            _unitOfWork = unitOfWork;
        }

        public void Remove(Address peopleAddress)
        {
            peopleAddress.IsActive = false;
            _repositoryAddress.Update(peopleAddress);
        }

        public void RemoveUow(Address peopleAddress)
        {
            Remove(peopleAddress);
            _unitOfWork.CommitAsync();
        }
    }
}