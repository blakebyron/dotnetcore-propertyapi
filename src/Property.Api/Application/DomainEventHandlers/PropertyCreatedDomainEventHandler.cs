using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Property.Core.Events;

namespace Property.Api.Application.DomainEventHandlers
{
    public class PropertyCreatedDomainEventHandler: INotificationHandler<PropertyCreated>
    {
        private readonly ILogger<PropertyCreatedDomainEventHandler> logger;

        public PropertyCreatedDomainEventHandler(ILogger<PropertyCreatedDomainEventHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(PropertyCreated notification, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Event Handler started for Property {0}", notification.Property.ID);
            this.logger.LogInformation("Event Handler Finished for Property {0}", notification.Property.ID);
            return Task.CompletedTask;
        }
    }
}
