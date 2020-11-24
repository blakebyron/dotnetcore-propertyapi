using System;
using System.Collections.Generic;
using System.Text;

namespace Property.Api.IntegrationTests.Features.Property
{
    public class PropertyBuilder
    {

        private PropertyWithReferenceAndDescription _entity;

        public PropertyWithReferenceAndDescription Build()
        {
            return _entity;
        }

        public PropertyBuilder Reference(string reference)
        {
            _entity.PropertyReference = reference;
            return this;
        }

        public PropertyBuilder Description(string description)
        {
            _entity.PropertyDescription = description;
            return this;
        }

        public PropertyBuilder WithTestValues()
        {
            _entity = new PropertyWithReferenceAndDescription
            {
                PropertyReference = "P9999",
                PropertyDescription = "The brand new property"
            };
            return this;
        }
    }

    public class PropertyWithReferenceAndDescription
    {
        public string PropertyReference { get; set; }
        public string PropertyDescription { get; set; }
    }

}
