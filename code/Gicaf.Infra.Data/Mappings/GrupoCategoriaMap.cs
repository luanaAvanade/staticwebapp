using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class GrupoCategoriaMap : BaseMap<GrupoCategoria>
    {
        public GrupoCategoriaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }

        public GrupoCategoriaMap():base(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<GrupoCategoria> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Codigo).HasMaxLength(10);
            builder.HasOne(x => x.Categoria).WithMany(x => x.Grupos).HasForeignKey(x => x.CategoriaId);
            builder.HasIndex(nameof(GrupoCategoria.Codigo), nameof(GrupoCategoria.CategoriaId)).IsUnique();
        }
    }
}
