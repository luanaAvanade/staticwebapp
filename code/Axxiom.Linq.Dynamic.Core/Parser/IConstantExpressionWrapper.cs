using System.Linq.Expressions;

namespace Axxiom.Linq.Dynamic.Core.Parser
{
    internal interface IConstantExpressionWrapper
    {
        void Wrap(ref Expression expression);
    }
}
