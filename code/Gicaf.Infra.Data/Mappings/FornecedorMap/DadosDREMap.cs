using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;

namespace Gicaf.Infra.Data.Mappings
{
    public class DadosDREMap : BaseMap<DadosDRE>
    {
        public DadosDREMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public DadosDREMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<DadosDRE> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Empresa).WithMany(x => x.DadosDREs).HasForeignKey(x => x.EmpresaId);
        }
    }
}