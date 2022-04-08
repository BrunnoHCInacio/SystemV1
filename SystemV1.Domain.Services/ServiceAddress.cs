using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Services
{
    public class ServiceAddress : Service, IServiceAddress
    {
        private readonly IRepositoryAddress _repositoryAddress;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceAddress(IRepositoryAddress repositoryAddress,
                              IUnitOfWork unitOfWork,
                              INotifier notifier) : base(notifier)
        {
            _repositoryAddress = repositoryAddress;
            _unitOfWork = unitOfWork;
        }

        public void Add(Address address)
        {
            _repositoryAddress.Add(address);
        }

        public async Task AddAsyncUow(Address address)
        {
            if (!IsValidEntity(address.ValidateAddress()))
            {
                return;
            }

            try
            {
                Add(address);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao adicionar o endereço.");
            }
        }

        public async Task<IEnumerable<Address>> GetAllAsync(int page, int pageSize)
        {
            try
            {
                return await _repositoryAddress.GetAllAddressesAsync(page, pageSize);
            }
            catch (Exception)
            {
                Notify("Falha ao obter todos os endereços.");
            }
            return null;
        }

        public async Task<Address> GetByIdAsync(Guid id)
        {
            try
            {
                return await _repositoryAddress.GetAddressByIdAsync(id);
            }
            catch (Exception)
            {
                Notify("Falha ao obter o endereço por id.");
            }
            return null;
        }

        public void Remove(Address address)
        {
            address.IsActive = false;
            Update(address);
        }

        public void RemoveAllByClientId(Guid clientId)
        {
            _repositoryAddress.RemoveAllByClientId(clientId);
        }

        public async Task RemoveAsyncUow(Address address)
        {
            try
            {
                Remove(address);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao remover o endereço.");
            }
        }

        public void Update(Address address)
        {
            _repositoryAddress.Update(address);
        }

        public async Task UpdateAsyncUow(Address address)
        {
            if (!IsValidEntity(address.ValidateAddress()))
            {
                return;
            }

            try
            {
                Update(address);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                Notify("Falha ao alterar o endereço");
            }
        }
    }
}