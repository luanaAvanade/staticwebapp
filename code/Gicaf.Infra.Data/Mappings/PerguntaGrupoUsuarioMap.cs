using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class PerguntaGrupoUsuarioMap : BaseMap<PerguntaGrupoUsuario>
    {
        public PerguntaGrupoUsuarioMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }

        public PerguntaGrupoUsuarioMap():base(new DefaultMapSettings())
        {
        }
        public override void Configure(EntityTypeBuilder<PerguntaGrupoUsuario> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Pergunta).WithMany(x => x.PerguntaGrupoUsuario).HasForeignKey(x => x.PerguntaId);
            builder.HasOne(x => x.GrupoUsuario).WithMany(x=>x.PerguntaGrupoUsuario).HasForeignKey(x => x.GrupoUsuarioId);
        }
    }
}
