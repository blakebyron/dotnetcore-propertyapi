﻿using System;
using System.Linq;
using System.Reflection;
using Autofac;
using FluentValidation;
using MediatR;
using af = Autofac.Module;

namespace Property.Api.Infrastructure.MediatR
{
    public class MediatRModule:af
    {
        private readonly Assembly assembly;

        public MediatRModule(Assembly assembly)
        {
            this.assembly = assembly;
        }

        protected override void Load(ContainerBuilder builder)
        {
            // Mediator itself
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            // request & notification handlers
            builder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });


            //register all the request handlers
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.GetTypeInfo()
                    .ImplementedInterfaces.Any(
                        i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            //register all the notification handlers
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.GetTypeInfo()
                .ImplementedInterfaces.Any(
                i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(INotificationHandler<>)))
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();


            // Register the Command's Validators (Validators based on FluentValidation library)
            builder
                .RegisterAssemblyTypes(assembly)
                .Where(t => t.IsClosedTypeOf(typeof(IValidator<>)))
                .AsImplementedInterfaces();

            builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            base.Load(builder);
        }
    }
}
