using Gicaf.Application.GraphQL.Models;
using Gicaf.Application.GraphQL.Resolvers;
using Gicaf.Application.GraphQL.Types;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Application.GraphQL
{
    public static class FieldTypeBuilder
    {
        public static FieldType For(EntityMetadata metaTable, string entityName, ResolverType resolverType)
        {
            entityName = GetFieldName(entityName, resolverType);
            var tableType = new EntityType(metaTable, entityName);
            return For(tableType, resolverType);
        }

        public static FieldType For(EntityType tableType, ResolverType resolverType)
        {
            IGraphType graphType = GetGraphType(resolverType, tableType);
            return For(graphType, tableType.EntityMetadata.Type, resolverType);
        }

        public static FieldType For(IGraphType resolvedType, Type entityType, ResolverType resolverType)
        {
            var entityName = GetFieldName(entityType.Name, resolverType);

            var table = new FieldType
            {
                Name = entityName,
                Type = resolvedType.GetType(),
                ResolvedType = resolvedType,
                Arguments = EntityType.GetArgumentsFor(entityType.Name, resolverType),
            };

            table.Metadata.Add(nameof(ResolverType), resolverType);
            table.Metadata.Add(nameof(Type), entityType);

            return table;
        }

        private static IGraphType GetGraphType(ResolverType resolverType, EntityType entityType)
        {
            switch (resolverType)
            {
                case ResolverType.FindById:
                case ResolverType.Insert:
                case ResolverType.Update:
                case ResolverType.Remove:
                    return entityType;
                case ResolverType.CountBy:
                case ResolverType.List:
                case ResolverType.InsertMany:
                case ResolverType.UpdateMany:
                case ResolverType.RemoveMany:
                case ResolverType.Aggregate:
                    return new ListGraphType(entityType);
                default:
                    return null;
            }
        }

        private static string GetFieldName(string entityName, ResolverType resolverType)
        {
            switch (resolverType)
            {
                case ResolverType.FindById:
                    return entityName;
                case ResolverType.CountBy:
                    return ../../"{entityName}{Constants.Patterns._count}";
                case ResolverType.Aggregate:
                    return ../../"{entityName}{Constants.Patterns._aggregate}";
                case ResolverType.List:
                    return ../../"{entityName}{Constants.Patterns._list}";
                case ResolverType.Insert:
                    return ../../"{entityName}{Constants.Patterns._insert}";
                case ResolverType.InsertMany:
                    return ../../"{entityName}{Constants.Patterns._insertMany}";
                case ResolverType.Update:
                    return ../../"{entityName}{Constants.Patterns._update}";
                case ResolverType.UpdateMany:
                    return ../../"{entityName}{Constants.Patterns._updateMany}";
                case ResolverType.Remove:
                    return ../../"{entityName}{Constants.Patterns._delete}";
                case ResolverType.RemoveMany:
                    return ../../"{entityName}{Constants.Patterns._deleteMany}";
                case ResolverType.ProcessarResultado:
                    return ../../"{entityName}_processar";
                case ResolverType.ProcessarMec:
                    return ../../"{entityName}_processarMec";
                case ResolverType.GerarRespostas:
                    return ../../"{entityName}_gerar";
                default:
                    return null;
            }
        }
    }
}
