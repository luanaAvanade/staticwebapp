using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class ArquivoMap : BaseMap<Arquivo>
    {
        public ArquivoMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public ArquivoMap() : this(new DefaultMapSettings())
        {
        }
        public override void Configure(EntityTypeBuilder<Arquivo> builder)
        {
            base.Configure(builder);
            builder.Ignore(x => x.Key);
            builder.Ignore(x => x.CaminhoCompleto);
            builder.Ignore(x => x.Conteudo);
            //builder.Property<OrigemArquivo>("Origem");
        }
    }
}
