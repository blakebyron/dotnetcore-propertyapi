using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Property.Infrastructure.Data;

namespace Property.Api.Features.PropertyCollection
{
    public class Create
    {

        public class Query : IRequest<Command>  
        {
        }

        public class Command : IRequest<int>
        {
            public IEnumerable<Property> Properties { get; }

            public Command(IEnumerable<Property> properties)
            {
                Properties = properties;
            }

            public class Property
            {
                public string PropertyReference { get; set; }
                public string PropertyDescription { get; set; }
            }
        }


        public class CommandHandler : IRequestHandler<Command, Int32>
        {
            private readonly PropertyContext context;

            public CommandHandler(PropertyContext propertyContext)
            {
                this.context = propertyContext;
            }

            public async Task<int> Handle(Command request, CancellationToken cancellationToken)
            {
                foreach (var item in request.Properties)
                {
                    var pr = new Core.PropertyReference(item.PropertyReference);
                    var p = Core.Property.CreateWithDescription(pr, item.PropertyDescription);
                    this.context.Properties.Add(p);
                }

                return await context.SaveChangesAsync();
            }
        }

    }
}
