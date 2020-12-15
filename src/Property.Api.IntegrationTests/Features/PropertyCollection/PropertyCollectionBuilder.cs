using System;
using System.Collections.Generic;
using System.Linq;
using Property.Api.IntegrationTests.Features.Property;

namespace Property.Api.IntegrationTests.Features.PropertyCollection
{
    public class PropertyCollectionBuilder
    {
        private List<PropertyWithReferenceAndDescription> _entityList;



        public PropertyCollectionBuilder()
        {
            _entityList = new List<PropertyWithReferenceAndDescription>();
        }

        public PropertyCollectionBuilder WithTestValues()
        {
            int i = _entityList.Count();
            i++;

            var entity = new PropertyWithReferenceAndDescription
            {
                PropertyReference = String.Format("P{0}",i),
                PropertyDescription = String.Format("The description of property {0}",i)
            };
            _entityList.Add(entity);
            return this;
        }

        public PropertyCollectionBuilder WithTestValues(string reference, string description)
        {
            var entity = new PropertyWithReferenceAndDescription
            {
                PropertyReference = reference,
                PropertyDescription = description
            };
            _entityList.Add(entity);
            return this;
        }


        public List<PropertyWithReferenceAndDescription> Build()
        {
            return _entityList;
        }
    }
}
