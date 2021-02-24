using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Property.Core.Events;

namespace Property.Api.Application.DomainEventHandlers
{
    public class PropertyDescriptionChangedEventHandler:INotificationHandler<PropertyDescriptionChanged>
    {
        private readonly ILogger<PropertyDescriptionChangedEventHandler> logger;

        public PropertyDescriptionChangedEventHandler(ILogger<PropertyDescriptionChangedEventHandler> logger)
        {
            this.logger = logger;
        }

        public Task Handle(PropertyDescriptionChanged notification, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Event Handler {0} started", this.GetType().FullName);

            this.logger.LogInformation("Description of Property {0} changed to {1}", notification.PropertyReference, notification.PropertyDescription);

            this.logger.LogInformation("Event Handler {0} finished", this.GetType().FullName);
            return Task.CompletedTask;
        }
    }
}
