using System;
using System.Linq;
using System.Reflection;
using af = Autofac.Module;
using AutoMapper;
using Autofac;

namespace Property.Api.Infrastructure.AutoMapper
{
    public class AutoMapperModule:af
    {
        private readonly Assembly assembly;

        public AutoMapperModule(Assembly assembly)
        {
            this.assembly = assembly;
        }

        protected override void Load(ContainerBuilder builder)
        {

            var profiles =
                from t in assembly.GetTypes()
                where typeof(Profile).IsAssignableFrom(t)
                select (Profile)Activator.CreateInstance(t);


            var config = new MapperConfiguration(config =>
            {
                foreach (var profile in profiles)
                {
                    config.AddProfile(profile);
                }

            });

            builder.RegisterInstance(config).As<MapperConfiguration>();

            base.Load(builder);
        }
    }
}
