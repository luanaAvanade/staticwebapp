using Gicaf.Domain.Entities;

namespace Gicaf.Application.GraphQL.Types.InputTypes
{
    public class ExigenciaCreateInput : BaseInputType<Exigencia>
    {
        public ExigenciaCreateInput()
        {
            Field(x => x.Nome);
        }
    }

    public class ExigenciaUpdateInput : BaseInputType<Exigencia>
    {
        public ExigenciaUpdateInput()
        {
            Field(x => x.Nome, true);
        }
    }
}