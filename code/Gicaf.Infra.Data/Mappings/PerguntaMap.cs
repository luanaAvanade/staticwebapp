using Gicaf.Domain.Entities;
using Gicaf.Infra.Data.DataSeeder;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class PerguntaMap : BaseMap<Pergunta>
    {
        public PerguntaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public PerguntaMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<Pergunta> builder)
        {
            base.Configure(builder);
            builder.HasIndex(x => x.Codigo).IsUnique();

            builder.HasData(InitialData.Perguntas);
        }
    }
}
