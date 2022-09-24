using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Interfaces.Services;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Application.GraphQL.Types.Base
{
    public abstract class GqlQueryFieldsBase<TServiceBase> :
        GqlFieldsBase<TServiceBase> where TServiceBase : IServiceBase

    {
        private static QueryArguments _baseListArgs =>
            new QueryArguments(
                new QueryArgument<StringGraphType> { Name = Constants.Arguments.alias },
                new QueryArgument<IntGraphType> { Name = Constants.Arguments.take },
                new QueryArgument<IntGraphType> { Name = Constants.Arguments.skip },
                new QueryArgument<StringGraphType> { Name = Constants.Arguments.where },
                new QueryArgument<StringGraphType> { Name = Constants.Arguments.orderBy },
                new QueryArgument<BooleanGraphType> { Name = Constants.Arguments.paged }
            );
        private static QueryArguments _baseFindByIdArgs =>
            new QueryArguments(
                new QueryArgument<NonNullGraphType<IdGraphType>> { Name = Constants.Arguments.id }
            );

        public GqlQueryFieldsBase(EntityType entityType):base(entityType)
        {
            AddMethod(nameof(IServiceBase.GetAllGeneric), _baseListArgs, new ListGraphType(entityType), _entityType.Name + Constants.Patterns._list);
            AddMethod(nameof(IServiceBase.GetGeneric), _baseFindByIdArgs, entityType, _entityType.Name);
            AddMethod(nameof(IServiceBase.CountBy), _baseFindByIdArgs, entityType, _entityType.Name);
        }
    }
}
