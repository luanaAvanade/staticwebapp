using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
namespace Gicaf.Infra.Data.Mappings
{
    public class ClassificacaoCNAEMap : BaseMap<ClassificacaoCNAE>
    {
        public ClassificacaoCNAEMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public ClassificacaoCNAEMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<ClassificacaoCNAE> builder)
        {
            base.Configure(builder);
        }
    }
}
