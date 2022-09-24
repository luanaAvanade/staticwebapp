using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class ValidadeDocumentoEstadoMap : BaseMap<ValidadeDocumentoEstado>
    {
        public ValidadeDocumentoEstadoMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }

        public ValidadeDocumentoEstadoMap(): this(new DefaultMapSettings())
        {
        }
        public override void Configure(EntityTypeBuilder<ValidadeDocumentoEstado> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.TipoDocumentoFuncionalidade).WithMany(x => x.ValidadeDocumentoEstado).HasForeignKey(x => x.TipoDocumentoFuncionalidadeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(x => x.Estado).WithMany().HasForeignKey(x => x.EstadoId);
            builder.HasIndex(x => new {x.TipoDocumentoFuncionalidadeId, x.EstadoId}).IsUnique();
        }
    }
}
