using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class TipoDocumentoFuncionalidadeMap : BaseMap<TipoDocumentoFuncionalidade>
    {
        public TipoDocumentoFuncionalidadeMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }

        public TipoDocumentoFuncionalidadeMap(): this(new DefaultMapSettings())
        {
        }
        public override void Configure(EntityTypeBuilder<TipoDocumentoFuncionalidade> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.TipoDocumento).WithMany(x => x.TipoDocumentoFuncionalidade).HasForeignKey(x => x.TipoDocumentoId);
            builder.HasIndex(x => new {x.TipoDocumentoId, x.Funcionalidade}).IsUnique();
        }
    }
}