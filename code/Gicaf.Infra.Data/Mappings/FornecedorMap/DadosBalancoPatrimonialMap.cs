using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;

namespace Gicaf.Infra.Data.Mappings
{
    public class DadosBalancoPatrimonialMap : BaseMap<DadosBalancoPatrimonial>
    {
        public DadosBalancoPatrimonialMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public DadosBalancoPatrimonialMap() : this(new DefaultMapSettings())
        {
        }

        public override void Configure(EntityTypeBuilder<DadosBalancoPatrimonial> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Empresa).WithMany(x => x.DadosBalancosPatrimoniais).HasForeignKey(x => x.EmpresaId);
        }
    }
}