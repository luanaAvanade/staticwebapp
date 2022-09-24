using Gicaf.Domain.Entities;
using Gicaf.Domain.Services;
using Gicaf.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Axxiom.Linq.Dynamic.Core;
using Axxiom.Linq.Dynamic.Core.CustomTypeProviders;
using System.Reflection;
using System.Text.RegularExpressions;
using Gicaf.Domain.Entities.Base;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Gicaf.Infra.Data
{
    public static class QueryableExtensions
    {
        const string SELECT_PATTERN = "new({0})";

        //static ParsingConfig parsingConfig;
        static QueryableExtensions()
        {
            //System.DateTime
            ParsingConfig.Default.CustomTypeProvider = new CustomTypeProvider();
            ParsingConfig.Default.ResolveTypesBySimpleName = false;
            ParsingConfig.Default.AllowNewToEvaluateAnyType = true;
            ParsingConfig.Default.TryResolveTypeFromCustomTypeProvider = true;
            ParsingConfig.Default.TryResolveTypeFromExpression = false;
            //ParsingConfig.Default.CustomTypesPrefix = "fn_";


            ParsingConfig.DefaultEFCore21.CustomTypeProvider = new CustomTypeProvider();
            ParsingConfig.DefaultEFCore21.ResolveTypesBySimpleName = false;
            ParsingConfig.DefaultEFCore21.AllowNewToEvaluateAnyType = true;
            ParsingConfig.DefaultEFCore21.TryResolveTypeFromCustomTypeProvider = true;
            ParsingConfig.DefaultEFCore21.TryResolveTypeFromExpression = false;
            //ParsingConfig.Default.CustomTypesPrefix = "fn_";

            //parsingConfig = new ParsingConfig();
            ///parsingConfig.CustomTypeProvider = new CustomTypeProvider2();

            //parsingConfig.AllowNewToEvaluateAnyType = true;
            //parsingConfig.ResolveTypesBySimpleName = true;
            //parsingConfig.DisableMemberAccessToIndexAccessorFallback = true;
            //parsingConfig.RenameParameterExpression = true;
            //parsingConfig.
        }

        public static int PaginationCount<TEntity>(this IQueryable query, IQueryNode node)
        {
            if(node.Where != null)
            {
                return query.Count(WhereParser.Where(node.Where.ToString()));
            }
            return query.Count();
        }

        public static PagedResult<TEntity> PageResult<TEntity>(this IQueryable query, IQueryNode node)
        {
            string selectFields = null;
            selectFields = string.Join(',', node.Selections.Select(x => GetSubSelectionFields(x, false, typeof(TEntity))));

            if (!string.IsNullOrWhiteSpace(node.Where as string))
            {
                query = query.Where(WhereParser.Where(node.Where.ToString()));
            }
            if (!string.IsNullOrWhiteSpace(node.OrderBy as string))
            {
                query = query.OrderBy(node.OrderBy.ToString());
            }
            //var selectRoot = "t_" + typeof(TEntity).FullName + SELECT_PATTERN;
            var result = query.Select<TEntity>(string.Format(SELECT_PATTERN, selectFields)).PageResult(node.Pagination.Page, node.Pagination.PageSize);
            node.Pagination.Count = result.PageCount;
            return result;
        }

        private static Expression<Func<TEntity, TEntity>> Parse<TEntity>(string expression)
        {
            var type = typeof(TEntity);
            var expr = DynamicExpressionParser.ParseLambda(ParsingConfig.DefaultEFCore21, type, type, expression);
            //var expr = DynamicExpressionParser.ParseLambda(type, type, expression);
            return (Expression<Func<TEntity, TEntity>>)expr;
        }

        public static IQueryable Select(this IQueryable query, IQueryNode node)
        {
            var type = query.GetType().GetGenericArguments().FirstOrDefault();
            string selectFields = null;
            selectFields = string.Join(',', node.Selections.Select(x => GetSubSelectionFields(x, false, type)));

            if (!string.IsNullOrWhiteSpace(node.Where as string))
            {
                query = query.Where(/*parsingConfig,*/ WhereParser.Where(node.Where.ToString()));
                //query = query.Where(WhereParser.Where(node.Where.ToString()));
            }

            if (node.Pagination == null)
            {
                if (node.Skip != null)
                {
                    query = query.Skip(node.Skip.Value);
                }

                if (node.Take != null)
                {
                    query = query.Take(node.Take.Value);
                }
            }

            if (!string.IsNullOrWhiteSpace(node.OrderBy as string))
            {
                query = query.OrderBy(node.OrderBy.ToString());
            }

            query = query.Select(string.Format(SELECT_PATTERN, selectFields));

            if (node.Pagination != null)
            {
                var paged = query.PageResult(node.Pagination.Page, node.Pagination.PageSize);
                node.Pagination.Count = paged.RowCount;
                query = paged.Queryable;
            }
            return query;
        }

        public static IDictionary<string, int> CountBy(this IQueryable query, string where, string groupBy)
        {
            if (!string.IsNullOrWhiteSpace(groupBy))
            {
                List<dynamic> groups = null;
                if (string.IsNullOrWhiteSpace(where))
                {
                    groups = query.GroupBy(groupBy).Select("new (Key, Count() as Count)").ToDynamicList();
                }
                else
                {
                    groups = query.Where(where).GroupBy(groupBy).Select("new (Key, Count() as Count)").ToDynamicList();
                }

                var groupsDic = new Dictionary<string, int>();
                foreach (var group in groups)
                {
                    groupsDic.Add(group.Key.ToString(), group.Count);
                }

                return groupsDic;
            }
            else
            {
                var keyValuePair = KeyValuePair.Create("total", string.IsNullOrWhiteSpace(where) ? query.Count() : query.Count(where));
                return new Dictionary<string, int>(new List<KeyValuePair<string, int>> { keyValuePair });
            }
        }
        public static object Aggregate<T>(this IQueryable<T> query, string where, string groupBy, params string[] functions)
        {
            object result = null;
            string functionsStr = null;

            functionsStr = string.Join(",", functions.Select(x => string.Format("{0} as {1}", x, x.Replace('(','_').Replace(")",""))));
   
            if(!string.IsNullOrWhiteSpace(where))
            {
                query = query.Where(where);
            }

            if(!string.IsNullOrWhiteSpace(groupBy))
            {
                result = query.GroupBy(groupBy).OrderBy("Key desc").Select(string.Format("new (Key as Group, new({0}) as Value)", functionsStr));
            }
            else
            {
                result = query.GroupBy(x => 1).OrderBy("Key desc").Select(string.Format("new (\"Total\" as Group, new({0}) as Value)", functionsStr));
            }

            return result;
        }

        public static IQueryable<TEntity> Select2<TEntity>(this IQueryable<TEntity> query, IQueryNode node)
        {
            var type = query.GetType().GetGenericArguments().FirstOrDefault();
            string selectFields = null;
            selectFields = string.Join(',', node.Selections.Select(x => GetSubSelectionFields(x,false, typeof(TEntity), null, true)));

            if (!string.IsNullOrWhiteSpace(node.Where as string))
            {
                query = query.Where(/*parsingConfig,*/ WhereParser.Where(node.Where.ToString()));
                //query = query.Where(WhereParser.Where(node.Where.ToString()));
            }

            if (node.Pagination == null)
            {
                if (node.Skip != null)
                {
                    query = query.Skip(node.Skip.Value);
                }

                if (node.Take != null)
                {
                    query = query.Take(node.Take.Value);
                }
            }

            if (!string.IsNullOrWhiteSpace(node.OrderBy as string))
            {
                query = query.OrderBy(node.OrderBy.ToString());
            }

            query = query.Select<TEntity>(string.Format(SELECT_PATTERN, selectFields));
            
            if (node.Pagination != null)
            {
                var paged = query.PageResult(node.Pagination.Page, node.Pagination.PageSize);
                node.Pagination.Count = paged.RowCount;
                query = paged.Queryable;
            }
            return query;
        }

        private static string GetSubSelectionFields(IQueryNode node, bool usePrefix, Type type, string parent = null, bool convert = false)
        {
            string result = null;
            var fieldName = node.Name;//Char.ToUpperInvariant(node.Name[0]) + node.Name.Substring(1);
            var fieldPath = string.IsNullOrEmpty(parent) ? fieldName : parent + "." + fieldName;

            if (node.HasSelection)
            {
                var fieldType = type.PropertyFromPath(fieldName).PropertyType;
                var isCollection = typeof(IEnumerable).IsAssignableFrom(fieldType) && fieldType != typeof(string);//typeof(IEnumerable).IsAssignableFrom(fieldType);
                var conversionType = isCollection ? fieldType.GetGenericArguments()[0] : fieldType;

                if (isCollection)
                {
                    //var toCollection = ../../"t_{nameof(DynamicFunctions)}.{nameof(DynamicFunctions.ToCollection)}({{0}})";
                    //result = string.Format(toCollection, "it." + fieldPath);

                    //result = fieldPath;//"it." + fieldPath;
                    result = usePrefix ? fieldPath : fieldName;

                    if (!string.IsNullOrWhiteSpace(node.Where as string))
                    {
                        result += ../../".Where({ WhereParser.Where(node.Where.ToString()) })";
                    }

                    if(node.Skip != null)
                    {
                        result += ../../".Skip({node.Skip})";
                    }

                    if (node.Take != null)
                    {
                        result += ../../".Take({node.Take})";
                    }

                    if (!string.IsNullOrWhiteSpace(node.OrderBy as string))
                    {
                        result += ../../".OrderBy({node.OrderBy})";
                    }
                    //result += string.Format(".Select(new {0} ({1})) as {2}", "t_" + conversionType.FullName, string.Join(",", node.Selections.Select(x => GetSubSelectionFields(x, true, conversionType, fieldName))), fieldName);
                    //result += " == null ? null : "+ result;
                    result += string.Format(".Select(new ({0})) as {1}", string.Join(",", node.Selections.Select(x => GetSubSelectionFields(x, false, conversionType, fieldName))), fieldName);
                }
                else
                {
                    if(convert)
                    {
                        if (usePrefix)
                        {
                            result = string.Format("{0} == null ? null : new {1} ({2}) as {3}", fieldPath, "t_" + conversionType.FullName, string.Join(",", node.Selections.Select(x => GetSubSelectionFields(x, false, conversionType, fieldPath))), fieldName);
                        }
                        else
                        {
                            result = string.Format("{0} == null ? null : new {1} ({2}) as {3}", fieldName, "t_" + conversionType.FullName, string.Join(",", node.Selections.Select(x => GetSubSelectionFields(x, false, conversionType, fieldName))), fieldName);
                        }
                    }
                    else
                    {
                        //result = string.Format("{0} == null ? null : new ({1}) as {2}", fieldName, string.Join(",", node.Selections.Select(x => GetSubSelectionFields(x, false, conversionType, fieldName))), fieldName);
                        if (usePrefix)
                        {
                            result = string.Format("{0} == null ? null : new ({1}) as {2}", fieldPath, string.Join(",", node.Selections.Select(x => GetSubSelectionFields(x, true, conversionType, fieldPath))), fieldName);
                        }
                        else
                        {
                            result = string.Format("{0} == null ? null : new ({1}) as {2}", fieldName, string.Join(",", node.Selections.Select(x => GetSubSelectionFields(x, true, conversionType, fieldName))), fieldName);
                        }
                    }
                }
            }
            else
            {
                if (usePrefix)
                {
                    result = fieldPath;
                }
                else
                {
                    result = fieldName;
                }
            }
            return result;
        }
    }
}



    public static class ReflectionExt
    {
    public static PropertyInfo PropertyFromPath(this Type type, string path)
    {
        PropertyInfo property = null;
        foreach (string propertyName in path.Split('.'))
        {
            property = type.GetProperty(propertyName);
            if(typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && property.PropertyType != typeof(string))
            {
                type = property.PropertyType.GetGenericArguments()[0];
            }
            else
            {
                type = property.PropertyType;
            }
        }
        return property;
    }
}

public static class PersistedTypes
{
    static List<Type> _types = new List<Type>();
    public static void Add(Type type)
    {
        _types.Add(type);
    }

    public static List<Type> List()
    {
        return _types;
    }
}

[DynamicLinqType]
public static class DynamicFunctions
{
    //public static DbFunctions Functions => EF.Functions;

    public static bool Like(string matchExpression, string pattern) => EF.Functions.Like(matchExpression, pattern);
    public static bool Like(string matchExpression, string pattern, string escapeCharacter) => EF.Functions.Like(matchExpression, pattern, escapeCharacter);

    public static ICollection<T> ToCollection<T>(IEnumerable<T> enumerable)
    {
        var collection = new Collection<T>();
        foreach (T item in enumerable)
        {
            collection.Add(item);
        }
        return collection;
    }
}


public class CustomTypeProvider : AbstractDynamicLinqCustomTypeProvider, IDynamicLinkCustomTypeProvider
{
    public string TypePrefixIdentifier { get; set; } = "t_";

    Dictionary<string, List<Assembly>> _prefixTypes = new Dictionary<string, List<Assembly>>();
    HashSet<Type> _types = new HashSet<Type>(); 
    public CustomTypeProvider()
    {
        _prefixTypes.Add("t_", new List<Assembly>() { Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(BaseTrailEntity)) });

        var set = new HashSet<Type>(FindTypesMarkedWithDynamicLinqTypeAttribute(new List<Assembly> { Assembly.GetExecutingAssembly() }));

        foreach (var type in PersistedTypes.List())
        {
            set.Add(type);
        }
        _types = set;
    }

    public HashSet<Type> GetCustomTypes()
    {
        return _types;
    }

    public Type ResolveType(string typeName)
    {
        foreach (var key in _prefixTypes.Keys)
        {
            if(typeName.StartsWith(key))
            {
                var assemblies = _prefixTypes[key];
                typeName = typeName.Remove(0, key.Count());
                return ResolveType(assemblies, typeName);
            }
        }

        return null;
    }

    public Type ResolveTypeBySimpleName(string typeName)
    {
        foreach (var key in _prefixTypes.Keys)
        {
            if (typeName.StartsWith(key))
            {
                var assemblies = _prefixTypes[key];
                typeName = typeName.Remove(0, key.Count());
                return ResolveTypeBySimpleName(assemblies, typeName);
            }
        }

        return null;
    }
}

public static class WhereParser
{
    const string LIKE_PATTERN = @"(\w+)(\s+like\s+)(""\S*"")";

    public static readonly string LIKE_REPLACEMENT = ../../"t_{nameof(DynamicFunctions)}.{nameof(DynamicFunctions.Like)}(../../1,../../3)";

    const string BETWEEN_PATTERN = @"(([\w\d.]* *)( between )(.*)( +and +)([\w\d.]*))";
    const string BETWEEN_REPLACEMENT = "(../../2 >= ../../4 and ../../2 <= ../../6)";
    public static string Where(string where)
    {
        where = ReplaceLike(where);
        where = ReplaceBetween(where);
        return where;
    }

    private static string ReplaceLike(string where)
    {
        return Regex.Replace(where, LIKE_PATTERN, LIKE_REPLACEMENT, RegexOptions.IgnoreCase);
    }

    private static string ReplaceBetween(string where)
    {
        return Regex.Replace(where, BETWEEN_PATTERN, BETWEEN_REPLACEMENT, RegexOptions.IgnoreCase);
    }
}