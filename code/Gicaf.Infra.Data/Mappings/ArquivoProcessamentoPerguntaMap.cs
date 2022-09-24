using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class ArquivoProcessamentoPerguntaMap : BaseMap<ArquivoProcessamentoPergunta>
    {
        public ArquivoProcessamentoPerguntaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public ArquivoProcessamentoPerguntaMap(): this(new DefaultMapSettings())
        {
        }
        public override void Configure(EntityTypeBuilder<ArquivoProcessamentoPergunta> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Pergunta).WithMany().HasForeignKey(x => x.PerguntaId);
            builder.HasOne(x => x.Arquivo).WithMany().HasForeignKey(x => x.ArquivoId);
            builder.Ignore(x => x.Tipo);
        }
    }
}
