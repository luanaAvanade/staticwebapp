using Gicaf.Domain.Entities;
using Gicaf.Infra.Data.DataSeeder;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class ExigenciaMap : BaseMap<Exigencia>
    {
        public ExigenciaMap() : this(new DefaultMapSettings()) { }
        public ExigenciaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings) { }

         public override void Configure(EntityTypeBuilder<Exigencia> builder)
        {
            base.Configure(builder);
            builder.HasIndex(x => x.Nome).IsUnique();
            builder.HasOne(x => x.TipoExigencia).WithMany().HasForeignKey(x => x.TipoExigenciaId);
        }

    }
}