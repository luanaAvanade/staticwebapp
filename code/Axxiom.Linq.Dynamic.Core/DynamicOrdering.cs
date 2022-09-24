using System.Linq.Expressions;

namespace Axxiom.Linq.Dynamic.Core
{
    internal class DynamicOrdering
    {
        public Expression Selector;
        public bool Ascending;
        public string MethodName;
    }
}