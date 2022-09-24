using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;

namespace Gicaf.Infra.Data.Mappings
{
    public class GrupoFornecimentoMap : BaseMap<GrupoFornecimento>
    {
        public GrupoFornecimentoMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }

        public GrupoFornecimentoMap():base(new DefaultMapSettings()){}

        public override void Configure(EntityTypeBuilder<GrupoFornecimento> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Empresa).WithMany(x => x.GruposFornecimento).HasForeignKey(x => x.EmpresaId);
            builder.HasOne(x => x.GrupoCategoria).WithMany(x => x.GruposFornecimento).HasForeignKey(x => x.GrupoCategoriaId);
        }
 
    }
}