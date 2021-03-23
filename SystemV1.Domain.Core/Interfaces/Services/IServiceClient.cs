﻿using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Domain.Entitys;

namespace SystemV1.Domain.Core.Interfaces.Services
{
    public interface IServiceClient : IService<Client>
    {
        void Remove(Client client);
        void RemoveUow(Client client);
    }
}