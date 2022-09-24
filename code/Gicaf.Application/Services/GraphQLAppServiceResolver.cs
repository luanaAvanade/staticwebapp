using Gicaf.Application.GraphQL.Resolvers;
using Gicaf.Application.Interface.Services;
using Gicaf.Domain.Entities;
using Gicaf.Domain.Interfaces.Repository;
using Gicaf.Domain.Interfaces.Services;
using Gicaf.Domain.Services;
using GraphQL;
using GraphQL.Conversion;
using GraphQL.Instrumentation;
using GraphQL.Language.AST;
using GraphQL.Types;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gicaf.Application.Services
{
    internal class GraphQLQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }

    public class ResultExtensions
    {
        public ResultExtensions()
        {
            Extensions = new Dictionary<string, object>();
        }
        public Dictionary<string, object> Extensions { get; set; }
    }
    

    public class GraphQLAppServiceResolver : IAppServiceResolver
    {
        protected Schema _schema;
        protected ResultExtensions _resultExtensions;
        protected IServiceProvider _serviceProvider;
        public GraphQLAppServiceResolver(Schema schema, ResultExtensions resultExtensions, IServiceProvider serviceProvider)
        {
            _schema = schema;
            _resultExtensions = resultExtensions;
            _serviceProvider = serviceProvider;
        }

        public IAppActionResult Resolve(object obj, IDictionary<string,object> context)
        {
            var query = ((JObject)obj).ToObject<GraphQLQuery>();//JObject.FromObject(obj).ToObject<GraphQLQuery>();
            var inputs = query.Variables.ToInputs();
            
            var fieldMiddlewareBuilder = new FieldMiddlewareBuilder();
            var resolver = new FieldResolver(_serviceProvider, _resultExtensions);
            fieldMiddlewareBuilder.Use(x => resolver.Resolve).ApplyTo(_schema);
            var result = new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.FieldMiddleware = fieldMiddlewareBuilder;
                _.Schema = _schema;
                _.Query = query.Query;
                _.OperationName = query.OperationName;
                _.Inputs = inputs;
                _.ExposeExceptions = true;
                _.UserContext = context;
                //_.ThrowOnUnhandledException = true;
            }).ConfigureAwait(false).GetAwaiter().GetResult();

            result.Extensions = _resultExtensions.Extensions;
            return new GraphQLActionResult(result);
        }

        public void SetResolverType(AppResolverType resolverType)
        {
            throw new NotImplementedException();
        }
    }
    public class GraphQLActionResult : IAppActionResult
    {
        ExecutionResult _executionResult;
        public IExecutionErrors Errors { get; set; }
        public object Data { get => _executionResult.Data; set => _executionResult.Data = value; }
        public string Query { get => _executionResult.Query; set => _executionResult.Query = value; }
        public bool ExposeExceptions { get => _executionResult.ExposeExceptions; set => _executionResult.ExposeExceptions = value; }
        public Dictionary<string, object> Extensions { get => _executionResult.Extensions; set => _executionResult.Extensions = value; }

        public GraphQLActionResult(ExecutionResult executionResult)
        {
            _executionResult = executionResult;
            if(_executionResult.Errors?.Any() ?? false)
            {
                Errors = new GraphQLExecutionErrors(executionResult.Errors);
            }
        }
    }

    public class GraphQLExecutionErrors : List<IExecutionError>, IExecutionErrors
    {
        public GraphQLExecutionErrors(ExecutionErrors executionErrors)
        {
            if(executionErrors != null && executionErrors.Any())
            {
                AddRange(executionErrors.Select(x => new GraphQLExecutionError(x)));
            }
        }
    }

    public class GraphQLExecutionError : IExecutionError
    {
        ExecutionError _executionError;

        public ErrorType Type => ErrorType.Exception;
        public GraphQLExecutionError(ExecutionError executionError)
        {
            _executionError = executionError;
        }
        public string Code { get => _executionError.Code; set => _executionError.Code = value; }
        public Exception InnerException => _executionError.InnerException;
        public string Message => _executionError.Message;
        public IEnumerable<string> Path { get => _executionError.Path; set => _executionError.Path = value; }
        public Dictionary<string, object> DataAsDictionary => _executionError.DataAsDictionary;
    }
}