using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Entities.Enums;
using System.Collections.Generic;

namespace Gicaf.Domain.Entities.Fornecedores
{
    public class FabricanteGrupoFornecimento : BaseTrailEntity
    {
        public string CNPJ { get; set; }

        public string NomeFabricante { get; set; }
        public StatusAprovacao StatusAprovacao { get; set; }
        public long? GrupoFornecimentoId { get; set; }
        public GrupoFornecimento GrupoFornecimento { get; set; }
    }
}