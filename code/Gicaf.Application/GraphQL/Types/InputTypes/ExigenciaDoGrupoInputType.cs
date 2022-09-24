using Gicaf.Domain.Entities;

namespace Gicaf.Application.GraphQL.Types.InputTypes
{
    public class ExigenciaGrupoQualificacaoCreateInput : BaseInputType<ExigenciaGrupoQualificacao>
    {
        public ExigenciaGrupoQualificacaoCreateInput()
        {

            Field(x => x.ExigenciaId);
            Field(x => x.GrupoCategoriaId);
            Field(x => x.Distribuidor);
            Field(x => x.Fabricante);
        }
    }

    public class ExigenciaGrupoQualificacaoUpdateInput : BaseInputType<ExigenciaGrupoQualificacao>
    {
        public ExigenciaGrupoQualificacaoUpdateInput()
        {
            Field(x => x.ExigenciaId, false, typeof(ExigenciaUpdateInput));
            Field(x => x.GrupoCategoriaId, false, typeof(GrupoCategoriaUpdateInput));
        }
    }
}