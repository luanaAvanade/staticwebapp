using Gicaf.Domain.Entities;
using GraphQL.Types;

namespace Gicaf.Application.GraphQL.Types.InputTypes
{
    public class ComentarioCreateInput : BaseInputType<Comentario>
    {
        public ComentarioCreateInput()
        {
            Field(x => x.EmpresaFornecedoraId, true);
            Field(x => x.Id, true);
            Field(x => x.Local, false, typeof(EnumerationGraphType<EnumItemAnalise>));
            Field(x => x.UsuarioId, true);
            Field(x => x.Usuario, true, typeof(UsuarioUpdateInput));
            Field(x => x.ArquivoId, true);
            Field(x => x.Coment);
        }
    }    
}