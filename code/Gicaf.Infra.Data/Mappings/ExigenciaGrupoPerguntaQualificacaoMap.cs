using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;

namespace Gicaf.Infra.Data.Mappings
{
    public class ExigenciaGrupoQualificacaoMap : BaseMap<ExigenciaGrupoQualificacao>
    {
        public ExigenciaGrupoQualificacaoMap() : this(new DefaultMapSettings()) { }
        public ExigenciaGrupoQualificacaoMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings) { }

         public override void Configure(EntityTypeBuilder<ExigenciaGrupoQualificacao> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Exigencia).WithMany(x=> x.ExigenciasGrupoQualificacao).HasForeignKey(x => x.ExigenciaId);
            builder.HasOne(x => x.GrupoCategoria).WithMany(x => x.ExigenciasGrupoPerguntaQualificacao).HasForeignKey(x => x.GrupoCategoriaId);
        }
    }
}