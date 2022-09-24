using System.Linq.Expressions;
using System.Reflection;

namespace Axxiom.Linq.Dynamic.Core.Parser.SupportedMethods
{
    internal class MethodData
    {
        public MethodBase MethodBase { get; set; }
        public ParameterInfo[] Parameters { get; set; }
        public Expression[] Args { get; set; }
    }
}
