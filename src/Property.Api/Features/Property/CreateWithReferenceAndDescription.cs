using System;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using pc = Property.Core;
using Property.Core.ValueObjects;
using Property.Infrastructure.Data;

namespace Property.Api.Features.Property
{
    public class CreateWithReferenceAndDescription
    {
        public class Query : IRequest<Command>
        {
        }

        public class Command : IRequest<Guid>
        {
            public string PropertyReference { get; set; }
            public string PropertyDescription { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(p => p.PropertyReference).NotNull().Length(1, 250).Must(p => p.StartsWith("P"));
            }
        }

        public class CommandHandler : IRequestHandler<Command, Guid>
        {
            private readonly PropertyContext context;

            public CommandHandler(PropertyContext propertyContext)
            {
                this.context = propertyContext;
            }

            public async Task<Guid> Handle(Command request, CancellationToken cancellationToken)
            {
                var pr = new PropertyReference(request.PropertyReference);
                var p = pc.Property.CreateWithDescription(pr,request.PropertyDescription);

                this.context.Properties.Add(p);
                await context.SaveEntityBaseAsync(cancellationToken);
                return p.ID;
            }
        }
    }
}
