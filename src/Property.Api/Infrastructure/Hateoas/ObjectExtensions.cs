﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Property.Api.Infrastructure.Hateoas
{
    public static class ObjectExtensions
    {
        public static ExpandoObject ShapeData(this object source, Type type)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var dataShapedObject = new ExpandoObject();


            // all public properties should be in the ExpandoObject 
            var propertyInfos = type
                    .GetProperties(BindingFlags.IgnoreCase |
                    BindingFlags.Public | BindingFlags.Instance);

            foreach (var propertyInfo in propertyInfos)
            {
                // get the value of the property on the source object
                var propertyValue = propertyInfo.GetValue(source);

                // add the field to the ExpandoObject
                ((IDictionary<string, object>)dataShapedObject)
                    .Add(propertyInfo.Name, propertyValue);
            }

            return dataShapedObject;



        }
    }
}
