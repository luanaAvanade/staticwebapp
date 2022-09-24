using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;

namespace Gicaf.Infra.Data.Mappings
{
    public class TipoExigenciaMap : BaseMap<TipoExigencia>
    {
        public TipoExigenciaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public TipoExigenciaMap() : this(new DefaultMapSettings())
        {
        }
        public override void Configure(EntityTypeBuilder<TipoExigencia> builder)
        {
            base.Configure(builder);
            builder.HasIndex(x => new { x.Nome, x.NivelExigencia });//.HasIndex(x => x.Nome).IsUnique();
        }
    }
}
