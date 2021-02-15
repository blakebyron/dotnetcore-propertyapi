using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Property.Infrastructure.Data;

namespace Property.Api.Features.Property
{
    public class PropertyPatchModel
    {
        public string PropertyDescription { get; set; }
    }

    public class Update
    {
 
        public class Command : IRequest<Result>
        {
            public string PropertyReference { get; }

            public JsonPatchDocument<PropertyPatchModel> JsonPatchDocument { get; set; }

            public Command(string propertyReference, JsonPatchDocument<PropertyPatchModel> jsonPatchDocument)
            {
                this.PropertyReference = propertyReference;
                JsonPatchDocument = jsonPatchDocument;
            }
        }

        public class Result
        {

        }

        public class CommandHandler : IRequestHandler<Command, Result>
        {
            private readonly PropertyContext context;

            public CommandHandler(PropertyContext propertyContext)
            {
                this.context = propertyContext;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                var originalProperty = this.context.Properties.SingleOrDefault(p => p.Reference.Reference == request.PropertyReference);

                if (originalProperty == null)
                    return null;


                //Convert original object to patch model
                var propertyPatch = new PropertyPatchModel();
                request.JsonPatchDocument.ApplyTo(propertyPatch,error=>
                {
                    //ToDo: Add some logic to handle errors with applying process
                    //error.Operation
                });

                originalProperty.ChangeDescription(propertyPatch.PropertyDescription);

                await this.context.SaveChangesAsync();
           
                return new Result();
            }
        }
    }
}
