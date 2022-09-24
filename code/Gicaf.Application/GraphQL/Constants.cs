using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Application.GraphQL
{
    public static class Constants
    {
        public static class Patterns
        {
            public const string _list = nameof(_list);
            public const string _insert = nameof(_insert);
            public const string _insertMany = nameof(_insertMany);
            public const string _update = nameof(_update);
            public const string _updateMany = nameof(_updateMany);
            public const string _delete = nameof(_delete);
            public const string _deleteMany = nameof(_deleteMany);
            public const string _count = nameof(_count);
            public const string _aggregate = nameof(_aggregate);
        }
        public static class Arguments
        {
            public const string alias = nameof(alias);
            public const string id = nameof(id);
            public const string ids = nameof(ids);
            public const string take = nameof(take);
            public const string skip = nameof(skip);
            public const string where = nameof(where);
            public const string orderBy = nameof(orderBy);
            public const string paged = nameof(paged);
            public const string input = nameof(input);
            public const string groupBy = nameof(groupBy);
            public const string codigoPergunta = nameof(codigoPergunta);
            public const string functions = nameof(functions);
        }
    }
}
