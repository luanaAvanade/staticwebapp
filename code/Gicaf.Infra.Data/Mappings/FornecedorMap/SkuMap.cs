using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;

namespace Gicaf.Infra.Data.Mappings {
    public class SkuMap : BaseMap<Sku> {
      
      public SkuMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public SkuMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<Sku> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.GrupoCategoria).WithMany(x => x.Skus).HasForeignKey(x => x.GrupoCategoriaId);
        }
    }
}