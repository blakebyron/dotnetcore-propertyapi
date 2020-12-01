using System;
using System.Collections.Generic;
using Autofac;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Property.Api.Infrastructure.AutoMapper;
using Property.Api.Infrastructure.EntityFramework;
using Property.Api.Infrastructure.MediatR;

namespace Property.Api.Infrastructure.Autofac
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// A centralised place for registering all interfaces and modules which help bring the app together
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterApplicationModules(this ContainerBuilder builder, IConfiguration configuration)
        {
            var asm = typeof(Startup).Assembly;
            builder.RegisterModule(new MediatRModule(asm));
            builder.RegisterModule(new AutoMapperModule(asm));
            builder.RegisterModule(new EntityFrameworkModule(configuration));
            builder.RegisterType<ActionContextAccessor>().As<IActionContextAccessor>();


        }
    }
}

