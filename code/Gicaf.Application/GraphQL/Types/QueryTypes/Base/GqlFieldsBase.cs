using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Interfaces.Services;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Application.GraphQL.Types.Base
{
    public abstract class GqlFieldsBase<TServiceBase>
        : ObjectGraphType where TServiceBase : IServiceBase
    {
        protected Type _entityType;

        public GqlFieldsBase(EntityType entityType)
        {
            _entityType = entityType.EntityMetadata.Type;
        }

        public void AddMethod(string methodName, QueryArguments queryArguments, IGraphType resolvedType, string graphqlName = null)
        {
            var field = new FieldType
            {
                Name = graphqlName ?? _entityType.Name + ../../"_{methodName}",
                Description = "",
                Type = resolvedType.GetType(),
                ResolvedType = resolvedType,
                Arguments = queryArguments,
                Metadata = new Dictionary<string, object>(),
                //Resolver = new BaseGqlResolver<TServiceBase>(null, typeof(TEntityType)),
            };

            var method = typeof(TServiceBase).GetMethod(methodName);
            field.Metadata.Add("Method", method);

            AddField(field);
        }
    }
}
