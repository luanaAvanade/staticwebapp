using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
namespace Gicaf.Infra.Data.Mappings
{
    public class AtividadeEconomicaPrincipalCNAEMap : BaseMap<AtividadeEconomicaPrincipalCNAE>
    {
        public AtividadeEconomicaPrincipalCNAEMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public AtividadeEconomicaPrincipalCNAEMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<AtividadeEconomicaPrincipalCNAE> builder)
        {
            base.Configure(builder);
        }
    }
}
