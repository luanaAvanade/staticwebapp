using Gicaf.Application.GraphQL.Types;
using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Interfaces.Services;
using Gicaf.Domain.Services;
using GraphQL.Resolvers;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Gicaf.Application.GraphQL.Resolvers.ServiceResolvers
{
    public class BaseGqlResolver<TServiceBase>: IFieldResolver where TServiceBase : IServiceBase
    {
        IServiceBase _serviceBase;
        NameFieldResolver _nameFieldResolver = new NameFieldResolver();
        Type _entityType;

        Dictionary<string, QueryArgument> _arquments;

        public BaseGqlResolver(IServiceBase serviceBase, Type entityType)
        {
            _serviceBase = serviceBase;
            _entityType = entityType;
        }

        public void Handle(Expression<Func<TServiceBase, Func<object, IDictionary<string,object>, object>>> expression)
        {
            var result = ((MethodCallExpression)expression.Body).Method.Invoke(new { }, null);
            Handle(x => x.AddGeneric);
        }

        public void Handle(Expression<Func<TServiceBase, object>> expression, params QueryArgument[] queryArguments)
        {
            var result = ((MethodCallExpression)expression.Body);
            var methodName = result.Method.Name;
            Handle(x => x.AddGeneric);
        }

        public object Resolve(ResolveFieldContext context)
        {
            if (context.Source != null)
            {
                return Task.FromResult(_nameFieldResolver.Resolve(context));
            }

            var methodName = ResolveMethodName(context);
            _serviceBase.GetType().GetMethod(methodName);
            QueryArguments arguments = GetArguments(context.Arguments);

            _serviceBase.SaveChanges();
            return Task.FromResult<object>(null);
        }

        private QueryArguments GetArguments(Dictionary<string, object> arguments)
        {
            List<QueryArgument> queryArguments = new List<QueryArgument>();

            foreach (var argument in arguments)
            {
                queryArguments.Add(_arquments[argument.Key]);
            }
            return new QueryArguments(queryArguments);
        }

        private string ResolveMethodName(ResolveFieldContext context)
        {
            //var entityBaseName = _entityType.Name;//typeof(IServiceBase).GetGenericArguments()[0].Name;
            var startIndex = context.FieldName.IndexOf('_') + 1;
            string methodName = context.FieldName.Substring(startIndex);
            methodName = char.ToUpperInvariant(methodName[0]) + methodName.Substring(1);
            return methodName;
        }
    }
}
