using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Application.Interface.Services
{

    public enum AppResolverType
    {
        GraphQl,
        OData
    }

    public enum ErrorType
    {
        Validation,
        Exception
    }

    public interface IExecutionErrors : IEnumerable<IExecutionError>, IEnumerable
    {
        IExecutionError this[int index] { get; }
        int Count { get; }
        void Add(IExecutionError error);
        void AddRange(IEnumerable<IExecutionError> errors);
    }

    public interface IExecutionError// : Exception
    {
        //public IEnumerable<ErrorLocation> Locations { get; }
        string Code { get; set; }
        string Message { get; }
        Exception InnerException { get; }
        IEnumerable<string> Path { get; set; }
        Dictionary<string, object> DataAsDictionary { get; }
        //public override IDictionary Data { get; }
        //public void AddLocation(int line, int column);
        ErrorType Type { get; }
    }

    public interface IAppActionResult
    {
        object Data { get; set; }
        IExecutionErrors Errors { get; set; }
        string Query { get; set; }
        //Document Document { get; set; }
        //Operation Operation { get; set; }
        //PerfRecord[] Perf { get; set; }
        bool ExposeExceptions { get; set; }
        Dictionary<string, object> Extensions { get; set; }
    }

    public interface IAppServiceResolver
    {
        void SetResolverType(AppResolverType resolverType);
        IAppActionResult Resolve(object obj, IDictionary<string, object> context);
        //IAppActionResult Result { get; set; }
    }
}
