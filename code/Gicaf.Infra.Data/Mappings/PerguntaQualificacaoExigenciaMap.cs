using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class PerguntaQualificacaoExigenciaMap : BaseMap<PerguntaQualificacaoExigencia>
    {
        public PerguntaQualificacaoExigenciaMap() : base(new DefaultMapSettings()) { }

        public PerguntaQualificacaoExigenciaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings) { }


        public override void Configure(EntityTypeBuilder<PerguntaQualificacaoExigencia> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.PerguntaQualificacao).WithMany(x => x.PerguntasQualificacaoExigencia).HasForeignKey(x => x.PerguntaQualificacaoId);
            builder.HasOne(x => x.Exigencia).WithMany(x => x.PerguntasQualificacaoExigencia).HasForeignKey(x => x.ExigenciaId);
        }
    }
}
