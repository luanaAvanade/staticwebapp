using Gicaf.Domain.Entities;

namespace Gicaf.Application.GraphQL.Types.InputTypes
{
    public class PerguntaQualificacaoExigenciaInput : BaseInputType<PerguntaQualificacaoExigencia>
    {
        public PerguntaQualificacaoExigenciaInput()
        {
            Field(x => x.PerguntaQualificacaoId, true);
            Field(x => x.ExigenciaId, true);
        }
    }
}