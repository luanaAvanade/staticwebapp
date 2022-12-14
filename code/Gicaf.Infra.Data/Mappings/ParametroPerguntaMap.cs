using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class ParametroPerguntaMap : BaseMap<ParametroPergunta>
    {
        public ParametroPerguntaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }

        public ParametroPerguntaMap():base(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<ParametroPergunta> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Pergunta).WithMany().HasForeignKey(x => x.PerguntaId);
            builder.HasIndex(x => x.Codigo).IsUnique();
        }
    }
}
