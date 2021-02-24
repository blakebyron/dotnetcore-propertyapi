using System;
using System.Collections.Generic;
using MediatR;

namespace Property.Core
{
    public abstract class EntityBase
    {
        public Guid ID { get; set; }

        public List<IDomainEvent> Events = new List<IDomainEvent>();
    }

    public class DomainEventBase : IDomainEvent
    {
        public DateTime EventDateTime => System.DateTime.UtcNow;
    }

    public interface IDomainEvent : INotification
    {
        public DateTime EventDateTime { get; }
    }
}
