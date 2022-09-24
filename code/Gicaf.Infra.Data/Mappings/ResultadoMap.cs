using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class ResultadoMap : BaseMap<Resultado>
    {
        public ResultadoMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }

        public ResultadoMap():this(new DefaultMapSettings())
        {
        }

        public override void Configure(EntityTypeBuilder<Resultado> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Pergunta).WithMany().HasForeignKey(x => x.PerguntaId);
            builder.HasOne(x => x.Categoria).WithMany().HasForeignKey(x => x.CategoriaId);
            builder.HasOne(x => x.ArquivoProcessamentoPergunta).WithMany().HasForeignKey(x => x.ArquivoProcessamentoPerguntaId);
            builder.HasOne(x => x.AvaliacaoCategoria).WithMany(x => x.Resultados).HasForeignKey(x => x.AvaliacaoCategoriaId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
