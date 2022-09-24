using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class CategoriaMap : BaseMap<Categoria>
    {
        public CategoriaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public CategoriaMap():this(new DefaultMapSettings())
        {
        }
        public override void Configure(EntityTypeBuilder<Categoria> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Codigo).HasMaxLength(10);
            builder.HasIndex(nameof(Categoria.Codigo), nameof(Categoria.VersaoMecId)).IsUnique();
            builder.HasOne(x => x.VersaoMec).WithMany(x => x.Categorias).HasForeignKey(x => x.VersaoMecId);
            //builder.HasMany(x => x.GruposCategoria).WithOne()
            //builder.OwnsMany()
        }
    }
}
