using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;

namespace Gicaf.Infra.Data.Mappings
{
    public class CalculoRiscoMap : BaseMap<CalculoRisco>
    {
        public CalculoRiscoMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public CalculoRiscoMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<CalculoRisco> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Empresa).WithMany(x => x.CalculoRiscoLista).HasForeignKey(x => x.EmpresaId);
        }
    }
}