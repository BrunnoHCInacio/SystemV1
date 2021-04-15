using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using SystemV1.Application;
using SystemV1.Application.Interfaces;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Services;
using SystemV1.Infrastructure.Data.Repositorys;

namespace SystemV1.Infrastructure.CrossCutting.IOC
{
    public class ConfigurationIOC
    {
        public static void Load(ContainerBuilder builder)
        {
            #region Application

            builder.RegisterType<ApplicationServiceProvider>().As<IApplicationServiceProvider>();
            builder.RegisterType<ApplicationServiceClient>().As<IApplicationServiceClient>();
            builder.RegisterType<ApplicationServiceState>().As<IApplicationServiceState>();

            #endregion Application

            #region Services

            builder.RegisterType<ProviderService>().As<IProviderService>();
            builder.RegisterType<ServiceAddress>().As<IServiceAddress>();
            builder.RegisterType<ServiceClient>().As<IServiceClient>();
            builder.RegisterType<ServiceContact>().As<IServiceClient>();
            builder.RegisterType<ServiceProduct>().As<IServiceProduct>();
            builder.RegisterType<ServiceProductItem>().As<IServiceProductItem>();
            builder.RegisterType<ServiceState>().As<IServiceState>();
            builder.RegisterType<ServiceCountry>().As<IServiceCountry>();

            #endregion Services

            #region Repositorys

            builder.RegisterType<RepositoryClient>().As<IRepositoryClient>();
            builder.RegisterType<RepositoryAddress>().As<IRepositoryAddress>();
            builder.RegisterType<RepositoryContact>().As<IRepositoryContact>();
            builder.RegisterType<RepositoryProduct>().As<IRepositoryProduct>();
            builder.RegisterType<RepositoryProductItem>().As<IRepositoryProductItem>();
            builder.RegisterType<RepositoryProvider>().As<IRepositoryProvider>();
            builder.RegisterType<RepositoryState>().As<IRepositoryState>();
            builder.RegisterType<RepositoryCountry>().As<IRepositoryCountry>();

            #endregion Repositorys
        }
    }
}