using GraphQL.Resolvers;
using GraphQL.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Gicaf.Application.GraphQL.Resolvers
{
    public class NameFieldResolver : IFieldResolver
    {
        public object Resolve(ResolveFieldContext context)
        {
            var source = context.Source;

            if (source == null || (source is string && string.IsNullOrWhiteSpace((source as string))))
            {
                return null;
            }

            //var name = Char.ToUpperInvariant(context.FieldAst.Name[0]) + context.FieldAst.Name.Substring(1);
            var name = context.FieldAst.Name;
            var value = GetPropValue(source, name);

            value = value ?? string.Empty;

            if (value == null)
            {
                throw new InvalidOperationException(../../"Expected to find property {context.FieldAst.Name} on {context.Source.GetType().Name} but it does not exist.");
            } 

            return value;
        }

        private static object GetPropValue(object src, string propName)
        {
            var property = src.GetType().GetProperty(propName);
            
            // Converter Enum para string / array de string no retorno
            
            // if(property.PropertyType.IsEnum)
            // {
            //     return HandleEnum(src, property);
            // }
            
            return property.GetValue(src, null);
        }

        private static object HandleEnum(object src, PropertyInfo property)
        {
            if (property.PropertyType.GetCustomAttributes(typeof(FlagsAttribute), true).Any())
            {
                var enumObj = (Enum)property.GetValue(src, null);
                var enumValues = Enum.GetValues(property.PropertyType);
                List<string> result = new List<string>();
                foreach (var item in enumValues)
                {
                    var enumValue = (Enum)item;
                    if (enumObj.HasFlag(enumValue))
                    {
                        result.Add(enumValue.ToString());
                    }
                }
                return result;
            }
            return property.GetValue(src, null).ToString();
        }
    }
}
