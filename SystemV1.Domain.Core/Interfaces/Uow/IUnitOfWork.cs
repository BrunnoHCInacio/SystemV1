using System;
using System.Collections.Generic;
using System.Text;

namespace SystemV1.Domain.Core.Interfaces.Uow
{
    public interface IUnitOfWork
    {
        void Commit();
        void RoolBack();
    }
}
