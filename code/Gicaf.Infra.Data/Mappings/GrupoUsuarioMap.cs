using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;

namespace Gicaf.Infra.Data.Mappings
{
    public class GrupoUsuarioMap : BaseMap<GrupoUsuario>
    {
        public GrupoUsuarioMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }

        public GrupoUsuarioMap() : this(new DefaultMapSettings())
        {

        }
        public override void Configure(EntityTypeBuilder<GrupoUsuario> builder)
        {
            base.Configure(builder);
        }
    }
}
