using Gicaf.Application.GraphQL.Models;
using Gicaf.Application.GraphQL.Resolvers;
using Gicaf.Application.GraphQL.Types;
using Gicaf.Application.Services;
using Gicaf.Domain.Entities;
using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Entities.Common;
using GraphQL.Builders;
using GraphQL.Language.AST;
using GraphQL.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gicaf.Application.GraphQL
{
    public static class GraphQLTypesBuilder
    {
        public static IEnumerable<EntityType> GetPersistedTypes(IDatabaseMetadata dbMetadata, ITableNameLookup tableNameLookup)
        {
            var metaTables = dbMetadata.GetTableMetadatas().Where(x => x.Type != typeof(Trail));
            List<EntityType> types = new List<EntityType>();

            foreach (var metaTable in metaTables)
            {
                var friendlyTableName = tableNameLookup.GetFriendlyName(metaTable.Name);
                types.Add(new EntityType(metaTable, friendlyTableName));
            }
            SetRelations(types);
            return types.Where(x => x.EntityMetadata.Type.IsSubclassOf(typeof(BaseTrailEntity)));
        }

        private static void SetRelations(List<EntityType> types)
        {
            foreach (var type in types)
            {
                foreach (var column in type.EntityMetadata.Columns.Where(x => !x.IsScalar && !typeof(IEnumerable).IsAssignableFrom(x.Type)))
                {
                    var columnType = types.First(x => x.EntityMetadata.Type == column.Type);
                    var field = type.Field(typeof(ObjectGraphType), column.ColumnName);
                    field.ResolvedType = columnType;
                }

                foreach (var column in type.EntityMetadata.Columns.Where(x => !x.IsScalar && typeof(IEnumerable).IsAssignableFrom(x.Type)))
                {
                    var genericColumnType = column.Type.GetGenericArguments()[0];
                    var columnType = types.First(x => x.EntityMetadata.Type == genericColumnType);

                    var field = type.Field(typeof(ListGraphType), column.ColumnName);
                    field.ResolvedType = new ListGraphType(columnType);
                    field.Arguments = EntityType.GetArgumentsFor(genericColumnType.Name, ResolverType.List);
                }
            } 
        }
    }

    public class GraphQLQuery : ObjectGraphType<object>
    {
        public GraphQLQuery(IEnumerable<EntityType> types)
        {
            var countColumns = new List<ColumnMetadata>
            {
                new ColumnMetadata { ColumnName = "Group", IsScalar = true, Type = typeof(string), DataType = "string" },
                new ColumnMetadata { ColumnName = "Count", IsScalar = true, Type = typeof(int), DataType = "int" }
            };

            var aggregateColumns = new List<ColumnMetadata>
            {
                new ColumnMetadata { ColumnName = "Group", IsScalar = true, Type = typeof(string), DataType = "string" },
                new ColumnMetadata { ColumnName = "Value", IsScalar = true, Type = typeof(object), DataType = "object" }
            };

            foreach (var type in types)
            {
                this.AddFieldFor(type, ResolverType.FindById);
                this.AddFieldFor(type, ResolverType.List);
                
                var countMetatable = new EntityMetadata { Columns = countColumns, Name = type.Name, Type = type.EntityMetadata.Type};
                var tableType = new EntityType(countMetatable, countMetatable.Name+Constants.Patterns._count);
                this.AddFieldFor(tableType, ResolverType.CountBy);

                var aggregateMetatable = new EntityMetadata { Columns = aggregateColumns, Name = type.Name, Type = type.EntityMetadata.Type };
                tableType = new EntityType(aggregateMetatable, aggregateMetatable.Name + Constants.Patterns._count);
                this.AddFieldFor(tableType, ResolverType.Aggregate);
            }
        }
    }

    public class GraphQLMutation : ObjectGraphType<object>
    {
        public GraphQLMutation(IEnumerable<EntityType> types)
        {
            foreach (var type in types)
            {
                Name = "Mutation";

                this.AddFieldFor(type, ResolverType.Insert);
                this.AddFieldFor(type, ResolverType.InsertMany);
                this.AddFieldFor(type, ResolverType.Update);
                this.AddFieldFor(type, ResolverType.UpdateMany);
                this.AddFieldFor(type, ResolverType.Remove);
                this.AddFieldFor(type, ResolverType.RemoveMany);

                if (type.Name == nameof(Resultado))
                {
                    var fieldType = new FieldType
                    {
                        Name = ../../"{type.Name}_processar",
                        Type = typeof(BooleanGraphType),
                        ResolvedType = new BooleanGraphType() { Name = "result" },//tableType,
                        Arguments = EntityType.GetArgumentsFor(type.Name, ResolverType.ProcessarResultado)
                    };
                    fieldType.Metadata.Add(nameof(ResolverType), ResolverType.ProcessarResultado);
                    fieldType.Metadata.Add(nameof(Type), type.EntityMetadata.Type);
                    AddField(fieldType);
                }

                if (type.Name == nameof(AvaliacaoCategoria))
                {
                    var fieldType = new FieldType
                    {
                        Name = ../../"{type.Name}_processarMec",
                        Type = typeof(BooleanGraphType),
                        ResolvedType = new BooleanGraphType() { Name = "result" },//tableType,
                    };
                    fieldType.Metadata.Add(nameof(ResolverType), ResolverType.ProcessarMec);
                    fieldType.Metadata.Add(nameof(Type), type.EntityMetadata.Type);
                    AddField(fieldType);
                }

                if (type.Name == nameof(Resposta))
                {
                    var fieldType = new FieldType
                    {
                        Name = ../../"{type.Name}_gerar",
                        Type = typeof(ListGraphType),
                        ResolvedType = new ListGraphType(type),
                        Arguments = EntityType.GetArgumentsFor(type.Name, ResolverType.GerarRespostas)
                    };
                    fieldType.Metadata.Add(nameof(ResolverType), ResolverType.GerarRespostas);
                    fieldType.Metadata.Add(nameof(Type), type.EntityMetadata.Type);
                    AddField(fieldType);
                }
            }
        }
    }
    public static class GraphExt
    {
        public static void AddFieldFor(this IComplexGraphType graph, EntityMetadata metaTable, string entityName, ResolverType resolverType)
        {
            var fieldType = FieldTypeBuilder.For(metaTable, entityName, resolverType);
            graph.AddField(fieldType);
        }
        public static void AddFieldFor(this IComplexGraphType graph, EntityType type, ResolverType resolverType)
        {
            var fieldType = FieldTypeBuilder.For(type, resolverType);
            graph.AddField(fieldType);
        }
    }
}


