using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Entitys;
using SystemV1.Domain.Services.Validations;

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
            if (!RunValidation(new AddressValidation(), address))
            {
                return;
            }

            Add(address);
            await _unitOfWork.CommitAsync();
        }

        public Task<IEnumerable<Address>> GetAllAsync(int page, int pageSize)
        {
            return _repositoryAddress.GetAllAsync(page, pageSize);
        }

        public Task<Address> GetByIdAsync(Guid id)
        {
            return _repositoryAddress.GetByIdAsync(id);
        }

        public void Remove(Address address)
        {
            address.IsActive = false;
            Update(address);
        }

        public async Task RemoveUow(Address address)
        {
            Remove(address);
            await _unitOfWork.CommitAsync();
        }

        public void Update(Address address)
        {
            _repositoryAddress.Update(address);
        }

        public async Task UpdateAsyncUow(Address address)
        {
            if (!RunValidation(new AddressValidation(), address))
            {
                return;
            }

            Update(address);
            await _unitOfWork.CommitAsync();
        }
    }
}