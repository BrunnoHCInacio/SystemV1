using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace SystemV1.Infrastructure.CrossCutting.IOC
{
    public class ModuleIOC : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            ConfigurationIOC.Load(builder);
        }
    }
}