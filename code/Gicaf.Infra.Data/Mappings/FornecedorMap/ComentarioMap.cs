using Gicaf.Domain.Entities.Fornecedores;
using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;

namespace Gicaf.Infra.Data.Mappings
{
    public class ComentarioMap : BaseMap<Comentario>
    {
        public ComentarioMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public ComentarioMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<Comentario> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Empresa).WithMany(x => x.Comentarios).HasForeignKey(x => x.EmpresaFornecedoraId);
            builder.HasOne(x => x.Arquivo).WithMany().HasForeignKey(x => x.ArquivoId);
            builder.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.UsuarioId);
        }
    }
}