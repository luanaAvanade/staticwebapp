using Gicaf.Domain.Entities.Base;
using GraphQL.Builders;
using GraphQL.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Gicaf.Application.GraphQL.Types.InputTypes
{
    public abstract class BaseInputType<TEntity>: InputObjectGraphType<TEntity> where TEntity : class
    {
        public FieldBuilder<TEntity, TProperty> FieldFromList<TProperty>(Expression<Func<TEntity, TProperty>> expression, bool nullable = false) where TProperty : IEnumerable
        {
            var listGenericType = typeof(TProperty).GetGenericArguments()[0];
            var listType = typeof(ListGraphType<>).
               MakeGenericType(typeof(ObjectGraphType<>).MakeGenericType(listGenericType));

            return Field(expression, nullable, listType);
        }
    }

    public abstract class DerivedInputType<TEntity, TDerivedInput> : BaseInputType<TEntity> where TEntity : class where TDerivedInput : IInputObjectGraphType
    {
        public DerivedInputType()
        {
            var derivedInput = Activator.CreateInstance<TDerivedInput>();
            foreach (var field in derivedInput.Fields)
            {
                AddField(field);
            }
        }
    }

    //Crirar input em tempo de execução
    public class BaseCreateInput<TEntity> : InputObjectGraphType<TEntity> //where TEntity: BaseEntity
    {
        public BaseCreateInput()
        {
            foreach (var prop in typeof(TEntity).GetProperties().Where(x => x.DeclaringType != typeof(BaseEntity)))
            {
                Type type = prop.PropertyType;
                if(type.IsEnum)
                {
                    type = typeof(EnumerationGraphType<>).MakeGenericType(type);
                }
                else if(type is IEnumerable)
                {
                    var genericType = type.GetGenericArguments()[0];
                    type = typeof(InputObjectGraphType).MakeGenericType(genericType);
                }

                type = prop.PropertyType;
                Field(type, prop.Name);
            }
        }
    }
}


public static class TypeExtensions
{
    public static bool ImplementsGenericInterface(this Type type, Type interfaceType)
    {
        if (interfaceType.IsGenericType)
        {
            return type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType);
        }
        return false;
        //throw new Exception("the value from interfaceType parameter is not valid");
    }

    public static bool ImplementsGenericInterface<TInterface>(this Type type)
    {
        return ImplementsGenericInterface(type, typeof(TInterface));
    }


    public static bool IsSubclassOfRawGeneric(this Type generic, Type toCheck)
    {
        while (generic != null)
        {
            if (generic.IsGenericType && generic.GetGenericTypeDefinition() == toCheck)
            {
                return true;
            }
            generic = generic.BaseType;
        }
        return false;
    }

    public static bool IsConcreteClass(this Type type)
    {
        return type.IsClass && !type.IsAbstract;
    }
}