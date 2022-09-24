using Gicaf.Domain.Entities;
using Gicaf.Infra.Data.DataSeeder;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;

namespace Gicaf.Infra.Data.Mappings
{
    public class TipoContatoMap : BaseMap<TipoContato>
    {
        public TipoContatoMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }

        public TipoContatoMap() : this(new DefaultMapSettings())
        {

        }
        public override void Configure(EntityTypeBuilder<TipoContato> builder)
        {
            base.Configure(builder);
            builder.HasData(InitialData.TiposContato);
        }
    }
}
