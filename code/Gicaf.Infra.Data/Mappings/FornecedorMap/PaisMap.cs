using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;

namespace Gicaf.Infra.Data.Mappings
{
    public class PaisMap : BaseMap<Pais>
    {
        public PaisMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public PaisMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<Pais> builder)
        {
            base.Configure(builder);
        }
    }
}
