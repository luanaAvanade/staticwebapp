using Gicaf.Domain.Entities.Base;

namespace Gicaf.Domain.Entities.Fornecedores
{

   public class Sku : BaseTrailEntity
    {

        public string Codigo {get; set;}
        public string Descricao {get; set;}
        public long GrupoCategoriaId  {get; set;}
        public GrupoCategoria GrupoCategoria {get; set;}
    }
}