using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Interfaces.Services;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Application.GraphQL.Types.Base
{
    public abstract class GqlMutationFieldsBase<TServiceBase>
        : GqlFieldsBase<TServiceBase> where TServiceBase : IServiceBase
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

        public GqlMutationFieldsBase(EntityType entityType, IGraphType defaultInputCreate = null, IGraphType defaultInputUpdate = null): base(entityType)
        {
            if (defaultInputCreate == null && defaultInputUpdate == null)
            {
                throw new Exception("");
            }

            if (defaultInputCreate != null)
            {
                AddMethod(nameof(IServiceBase.AddGeneric), new QueryArguments(new QueryArgument(defaultInputCreate)), entityType, _entityType.Name + Constants.Patterns._insert);
                AddMethod(nameof(IServiceBase.AddRangeGeneric), new QueryArguments(new QueryArgument(new ListGraphType(defaultInputCreate))), entityType, _entityType.Name + Constants.Patterns._insertMany);
            }

            if (defaultInputUpdate != null)
            {
                AddMethod(nameof(IServiceBase.UpdateGeneric), new QueryArguments(new QueryArgument(defaultInputUpdate)), entityType, _entityType.Name + Constants.Patterns._update);
                AddMethod(nameof(IServiceBase.UpdateRangeGeneric), new QueryArguments(new QueryArgument(new ListGraphType(defaultInputUpdate))), entityType, _entityType.Name + Constants.Patterns._updateMany);
            }

            var idsArgument = new QueryArguments(
               new QueryArgument<NonNullGraphType<IdGraphType>> { Name = Constants.Arguments.id }
           );

            AddMethod(nameof(IServiceBase.RemoveGeneric), idsArgument, entityType, _entityType.Name + Constants.Patterns._update);
        }
    }
}
