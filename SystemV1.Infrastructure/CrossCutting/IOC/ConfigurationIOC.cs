using Autofac;
using SystemV1.Application;
using SystemV1.Application.Interfaces;
using SystemV1.Application.Interfaces.Mapper;
using SystemV1.Application.Mappers;
using SystemV1.Domain.Core.Interfaces.Repositorys;
using SystemV1.Domain.Core.Interfaces.Services;
using SystemV1.Domain.Core.Interfaces.Uow;
using SystemV1.Domain.Core.Interfaces.Validations;
using SystemV1.Domain.Services;
using SystemV1.Domain.Services.Notifications;
using SystemV1.Domain.Services.Validations;
using SystemV1.Infrastructure.Data.Repositorys;
using SystemV1.Infrastructure.Data.Uow;

namespace SystemV1.Infrastructure.CrossCutting.IOC
{
    public class ConfigurationIOC
    {
        public static void Load(ContainerBuilder builder)
        {
            #region Repositorys

            builder.RegisterType<RepositoryClient>().As<IRepositoryClient>();
            builder.RegisterType<RepositoryAddress>().As<IRepositoryAddress>();
            builder.RegisterType<RepositoryContact>().As<IRepositoryContact>();
            builder.RegisterType<RepositoryProduct>().As<IRepositoryProduct>();
            builder.RegisterType<RepositoryProductItem>().As<IRepositoryProductItem>();
            builder.RegisterType<RepositoryProvider>().As<IRepositoryProvider>();
            builder.RegisterType<RepositoryState>().As<IRepositoryState>();
            builder.RegisterType<RepositoryCountry>().As<IRepositoryCountry>();
            builder.RegisterType<RepositoryCity>().As<IRepositoryCity>();
            builder.RegisterType<RepositoryPeople>().As<IRepositoryPeople>();

            #endregion Repositorys

            #region Services

            builder.RegisterType<ProviderService>().As<IProviderService>();
            builder.RegisterType<ServiceClient>().As<IServiceClient>();
            builder.RegisterType<ServiceAddress>().As<IServiceAddress>();
            builder.RegisterType<ServiceContact>().As<IServiceContact>();
            builder.RegisterType<ServiceProduct>().As<IServiceProduct>();
            builder.RegisterType<ServiceState>().As<IServiceState>();
            builder.RegisterType<ServiceCountry>().As<IServiceCountry>();
            builder.RegisterType<ServiceCity>().As<IServiceCity>();
            builder.RegisterType<ServiceProductItem>().As<IServiceProductItem>();

            #endregion Services

            #region Mappers

            builder.RegisterType<MapperAddress>().As<IMapperAddress>();
            builder.RegisterType<MapperClient>().As<IMapperClient>();
            builder.RegisterType<MapperContact>().As<IMapperContact>();
            builder.RegisterType<MapperCountry>().As<IMapperCountry>();
            builder.RegisterType<MapperProduct>().As<IMapperProduct>();
            builder.RegisterType<MapperProductItem>().As<IMapperProductItem>();
            builder.RegisterType<MapperProvider>().As<IMapperProvider>();
            builder.RegisterType<MapperState>().As<IMapperState>();
            builder.RegisterType<MapperCity>().As<IMapperCity>();

            #endregion Mappers

            #region Application

            builder.RegisterType<ApplicationServiceProvider>().As<IApplicationServiceProvider>();
            builder.RegisterType<ApplicationServiceClient>().As<IApplicationServiceClient>();
            builder.RegisterType<ApplicationServiceState>().As<IApplicationServiceState>();
            builder.RegisterType<ApplicationServiceNotifier>().As<IApplicationServiceNotifier>();
            builder.RegisterType<ApplicationServiceCountry>().As<IApplicationServiceCountry>();
            builder.RegisterType<ApplicationServiceCity>().As<IApplicationServiceCity>();

            #endregion Application

            #region Validations

            builder.RegisterType<ValidationCountry>().As<IValidationCountry>();
            builder.RegisterType<ValidationState>().As<IValidationState>();
            builder.RegisterType<ValidationCity>().As<IValidationCity>();
            builder.RegisterType<ValidationPeople>().As<IValidationPeople>();
            builder.RegisterType<ValidationClient>().As<IValidationClient>();
            builder.RegisterType<ValidationProvider>().As<IValidationProvider>();
            builder.RegisterType<ValidationProduct>().As<IValidationProduct>();
            builder.RegisterType<ValidationProductItem>().As<IValidationProductItem>();
            builder.RegisterType<ValidationAddress>().As<IValidationAddress>();
            builder.RegisterType<ValidationContact>().As<IValidationContact>();

            #endregion Validations

            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>();
            builder.RegisterType<Notifier>().As<INotifier>().InstancePerLifetimeScope();
        }
    }
}