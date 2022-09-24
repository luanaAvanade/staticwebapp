using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class RespostaMap : BaseMap<Resposta>
    {
        public RespostaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }

        public RespostaMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<Resposta> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Pergunta).WithMany(x => x.Respostas).HasForeignKey(x => x.PerguntaId);
            builder.HasOne(x => x.Usuario).WithMany(x => x.Respostas).HasForeignKey(x => x.UsuarioId);
            builder.HasOne(x => x.Categoria).WithMany().HasForeignKey(x => x.CategoriaId);

            builder.HasIndex(nameof(Resposta.CategoriaId), nameof(Resposta.PerguntaId), nameof(Resposta.UsuarioId)).IsUnique();
        }
    }
}
