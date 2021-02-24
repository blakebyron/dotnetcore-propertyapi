using System;
namespace Property.Core.ValueObjects
{
    public class UniqueDeliveryPointReferenceNumber
    {
        public string UDPRN { get; private set; }

        public UniqueDeliveryPointReferenceNumber(string udprn)
        {
            this.UDPRN = udprn;
        }
    }
}
