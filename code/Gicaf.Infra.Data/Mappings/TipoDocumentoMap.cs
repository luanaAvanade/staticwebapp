using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class TipoDocumentoMap : BaseMap<TipoDocumento>
    {
        public TipoDocumentoMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public TipoDocumentoMap() : this(new DefaultMapSettings())
        {
        }
        public override void Configure(EntityTypeBuilder<TipoDocumento> builder)
        {
            base.Configure(builder);
            builder.HasIndex(x => x.Nome).IsUnique();
        }
    }
}
