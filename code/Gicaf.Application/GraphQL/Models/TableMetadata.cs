using Gicaf.Application.GraphQL.Resolvers;
using Gicaf.Domain.Services;
using GraphQL.Language.AST;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gicaf.Application.GraphQL.Models
{
    public class EntityMetadata
    {
        public string Name { get; set; }

        public IEnumerable<ColumnMetadata> Columns { get; set; }

        public Type Type { get; set; }
    }
}
