using System;
using System.Collections.Generic;
using Autofac;
using Property.Api.Infrastructure.MediatR;

namespace Property.Api.Infrastructure.Autofac
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterApplicationModules(this ContainerBuilder builder)
        {
            var asm = typeof(Startup).Assembly;
            builder.RegisterModule(new MediatRModule(asm));
        }
    }
}

