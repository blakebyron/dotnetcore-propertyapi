using System;
namespace Property.Core.Events
{
    public class PropertyCreated:DomainEventBase
    {
        public Property Property { get; }

        public PropertyCreated(Property property)
        {
            Property = property;
        }

    }
}
