using System;

namespace Property.Core
{
    public class Property
    {
        public Int32 ID { get; set; }
        public PropertyReference Reference { get; private set; }
        public string Description { get; private set; }

        protected Property()
        {

        }

        protected Property(PropertyReference reference, string description)
        {
            this.Reference = reference;
            this.Description = description;
        }

        public static Property CreateWithDescription(PropertyReference reference, string description)
        {
            return new Property(reference, description);
        }
    }

    public class PropertyReference
    {
        public string Reference { get; private set; }

        public PropertyReference(string reference)
        {
            this.Reference = reference;
        }
    }
}
