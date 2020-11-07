using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Property.Infrastructure.Data;

namespace Property.Api.Features.Property
{
    public class CreateWithReferenceAndDescription
    {
        public class Query : IRequest<Command>
        {
        }

        public class Command : IRequest<int>
        {
            public string PropertyReference { get; set; }
            public string PropertyDescription { get; set; }
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
                var pr = new Core.PropertyReference(request.PropertyReference);
                var p = Core.Property.CreateWithDescription(pr,request.PropertyDescription);

                this.context.Properties.Add(p);
                return await context.SaveChangesAsync();
            }
        }
    }
}
