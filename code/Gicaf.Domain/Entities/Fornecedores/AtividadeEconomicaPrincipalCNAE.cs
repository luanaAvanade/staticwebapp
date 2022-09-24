using Gicaf.Domain.Entities.Base;
using System.Collections.Generic;

namespace Gicaf.Domain.Entities.Fornecedores
{
    public class AtividadeEconomicaPrincipalCNAE : BaseTrailEntity
    {  
        public string Codigo { get; set; }
        public string Descricao { get; set; }

        public ICollection<OcupacaoPrincipalCNAE> Ocupacoes {get; set;}
    }
}