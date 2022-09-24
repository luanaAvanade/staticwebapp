using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class GrupoPerguntaQualificacaoMap : BaseMap<GrupoPerguntaQualificacao>
    {
        public GrupoPerguntaQualificacaoMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public GrupoPerguntaQualificacaoMap() : this(new DefaultMapSettings())
        {
        }
        public override void Configure (EntityTypeBuilder<GrupoPerguntaQualificacao> builder)
        {
             base.Configure(builder);
            builder.HasIndex(x => x.Nome).IsUnique();
        }
    }
}