using Gicaf.Domain.Entities.Base;
using System.Collections.Generic;

namespace Gicaf.Domain.Entities.Fornecedores
{
    public enum Status {
        Ativo,
        Inativo
    }

    public enum TipoFornecimento
    {
        Fabricante,
        Distribuidor
    }
    
    public class GrupoFornecimento : BaseTrailEntity
    {
        public Empresa Empresa {get; set;}
 
        public long EmpresaId  {get; set;}

        public GrupoCategoria GrupoCategoria {get; set;}

        public long GrupoCategoriaId  {get; set;}

        public TipoFornecimento TipoFornecimento { get; set; }
        public ICollection<FabricanteGrupoFornecimento> FabricantesGrupoFornecimento { get; set; }
    }
}