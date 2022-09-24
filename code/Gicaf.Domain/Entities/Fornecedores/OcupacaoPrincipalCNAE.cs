using Gicaf.Domain.Entities.Base;

namespace Gicaf.Domain.Entities.Fornecedores
{
    public class OcupacaoPrincipalCNAE : BaseTrailEntity
    {  
       public string Codigo { get; set; }
        public string Descricao { get; set; }
        public long AtividadeEconomicaPrincipalCNAEId { get; set; }
        public AtividadeEconomicaPrincipalCNAE AtividadeEconomicaPrincipalCNAE { get; set; }

    }
}