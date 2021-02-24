using System;
namespace Property.Core.Events
{
    public class PropertyDescriptionChanged: DomainEventBase
    {
        public Guid PropertyID { get; set; }
        public string PropertyDescription { get; }
        public string PropertyReference { get; set; }
        public PropertyDescriptionChanged(Guid PropertyID, string propertyReference, string propertydescription)
        {
            this.PropertyDescription = propertydescription;
        }
    }
}
