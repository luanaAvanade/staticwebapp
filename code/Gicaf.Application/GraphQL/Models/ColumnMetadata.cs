using System;

namespace Gicaf.Application.GraphQL.Models
{
    public class ColumnMetadata
    {
        public EntityMetadata TableMetadata { get; set; }

        public string ColumnName { get; set; }

        public string DataType { get; set; }

        public bool IsScalar { get; set; }

        public Type Type { get; set; }

        public Type BaseType { get; set; }
    }
}
