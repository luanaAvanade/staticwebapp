using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;

namespace Gicaf.Infra.Data.Mappings
{
    public class FabricanteGrupoFornecimentoMap : BaseMap<FabricanteGrupoFornecimento>
    {
        public FabricanteGrupoFornecimentoMap() : this(new DefaultMapSettings()) { }
        public FabricanteGrupoFornecimentoMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings) { }

        public override void Configure(EntityTypeBuilder<FabricanteGrupoFornecimento> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.GrupoFornecimento).WithMany(x => x.FabricantesGrupoFornecimento).HasForeignKey(x => x.GrupoFornecimentoId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}