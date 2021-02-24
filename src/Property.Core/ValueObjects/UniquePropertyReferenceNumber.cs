using System;
namespace Property.Core.ValueObjects
{
    public class UniquePropertyReferenceNumber
    {
        public string UPRN { get; private set; }

        public UniquePropertyReferenceNumber(string uprn)
        {
            this.UPRN = uprn;
        }
    }
}
