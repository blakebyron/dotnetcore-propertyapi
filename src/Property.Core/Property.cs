using System;
using Property.Core.Events;
using Property.Core.ValueObjects;

namespace Property.Core
{
    public class Property:EntityBase
    {
        public PropertyReference Reference { get; private set; }
        public string Description { get; private set; }
        //public UniquePropertyReferenceNumber UPRN { get; private set; }
        //public UniqueDeliveryPointReferenceNumber UDPRN { get; private set; }

        protected Property()
        {

        }

        protected Property(PropertyReference reference, string description)
        {
            this.Reference = reference; 
            this.Description = description;
            //this.UPRN = new UniquePropertyReferenceNumber(String.Empty);
            //this.UDPRN = new UniqueDeliveryPointReferenceNumber(String.Empty);
            this.Events.Add(new PropertyCreated(this));

        }

        public static Property CreateWithDescription(PropertyReference reference, string description)
        {
            return new Property(reference, description);
        }

        public void ChangeDescription(string propertyDescription)
        {
            if (!this.Description.Equals(propertyDescription))
            {
                this.Description = propertyDescription;
                this.Events.Add(new PropertyDescriptionChanged(this.ID,this.Reference.Reference, this.Description));
            }
        }
    }
}
