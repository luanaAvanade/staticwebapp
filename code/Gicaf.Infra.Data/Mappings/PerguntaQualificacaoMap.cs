using Gicaf.Domain.Entities;
using Gicaf.Infra.Data.DataSeeder;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class PerguntaQualificacaoMap : BaseMap<PerguntaQualificacao>
    {
        public PerguntaQualificacaoMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public PerguntaQualificacaoMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<PerguntaQualificacao> builder)
        {
            base.Configure(builder);
        }
    }
}
