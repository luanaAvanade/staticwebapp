using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Gicaf.Infra.Data.Mappings.Base;
using Microsoft.EntityFrameworkCore;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class TermosAceiteMap : BaseMap<TermosAceite>
    {
        public TermosAceiteMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        
        public TermosAceiteMap() : this(new DefaultMapSettings())
        {
        }

        public override void Configure(EntityTypeBuilder<TermosAceite> builder)
        {
          base.Configure(builder);
          builder.Property(x => x.Descricao).HasColumnType("text").HasMaxLength(5000);
        }
    }
}
