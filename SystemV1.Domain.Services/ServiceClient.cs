using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceClient : Service<Client>, IServiceClient
    {
        private readonly IRepositoryClient _repositoryClient;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceClient(IRepositoryClient repositoruClient, 
                             IUnitOfWork unitOfWork):base(repositoruClient, unitOfWork)
        {
            _repositoryClient = repositoruClient;
            _unitOfWork = unitOfWork;
        }

        public void Remove(Client client)
        {
            client.IsActive = false;
            _repositoryClient.Update(client);
        }

        public void RemoveUow(Client client)
        {
            Remove(client);
            _unitOfWork.Commit();
        }
    }
}
