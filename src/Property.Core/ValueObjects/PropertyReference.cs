using System;
namespace Property.Core.ValueObjects
{
    public class PropertyReference
    {
        public string Reference { get; private set; }

        public PropertyReference(string reference)
        {
            this.Reference = reference;
        }
    }
}
