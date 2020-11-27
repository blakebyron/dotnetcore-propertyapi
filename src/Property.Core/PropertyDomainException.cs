using System;
using System.Collections.Generic;
using System.Text;

namespace Property.Core
{
    public class PropertyDomainException : Exception
    {
        public PropertyDomainException()
        { 
        
        }

        public PropertyDomainException(string message) : base(message)
        { 
        
        }

        public PropertyDomainException(string message, Exception innerException) : base(message, innerException)
        { 
        
        }
    }
}
