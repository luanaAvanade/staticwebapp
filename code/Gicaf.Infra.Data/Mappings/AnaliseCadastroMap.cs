using Gicaf.Domain.Entities;
using Gicaf.Infra.Data.Mappings.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;

namespace Gicaf.Infra.Data.Mappings
{
    public class AnaliseCadastroMap : BaseMap<AnaliseCadastro>
    {
        public override void Configure(EntityTypeBuilder<AnaliseCadastro> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.EmpresaFornecedora).WithOne(x => x.AnaliseCadastro).HasForeignKey<AnaliseCadastro>(x => x.EmpresaFornecedoraId);
            builder.HasOne(x => x.Atribuido).WithMany().HasForeignKey(x => x.AtribuidoId);
            builder.HasOne(x => x.Transmitido).WithMany().HasForeignKey(x => x.TransmitidoId);
            builder.HasIndex(x => x.EmpresaFornecedoraId).IsUnique();
        }
    }

    public class ItemAnaliseMap : BaseMap<ItemAnalise>
    {
      public override void Configure(EntityTypeBuilder<ItemAnalise> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Analise).WithMany(x => x.ItensAnalise).HasForeignKey(x => x.AnaliseId);
            builder.HasOne(x => x.Autor).WithMany().HasForeignKey(x => x.AutorId);
            builder.HasOne(x => x.Arquivo).WithOne().HasForeignKey<ItemAnalise>(x => x.ArquivoId);
            builder.HasIndex(x => new{x.AnaliseId, x.TipoItem, x.ArquivoId}).IsUnique();
        }
    }
}
