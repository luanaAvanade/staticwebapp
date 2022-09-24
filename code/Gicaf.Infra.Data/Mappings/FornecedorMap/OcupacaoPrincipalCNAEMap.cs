using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;


namespace Gicaf.Infra.Data.Mappings
{
    public class OcupacaoPrincipalCNAEMap : BaseMap<OcupacaoPrincipalCNAE>
    {
        public OcupacaoPrincipalCNAEMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public OcupacaoPrincipalCNAEMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<OcupacaoPrincipalCNAE> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.AtividadeEconomicaPrincipalCNAE).WithMany(x => x.Ocupacoes).HasForeignKey(x => x.AtividadeEconomicaPrincipalCNAEId);
        }
    }
}
