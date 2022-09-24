using Gicaf.Domain.Entities;
using Gicaf.Infra.Data.Mappings.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;

namespace Gicaf.Infra.Data.Mappings
{
    public class SocioMap : BaseMap<Socio>
    {
        public override void Configure(EntityTypeBuilder<Socio> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.EmpresaFornecedora).WithMany(x => x.Socios).HasForeignKey(x => x.EmpresaFornecedoraId).OnDelete(DeleteBehavior.Cascade);
            builder.HasIndex(x => new {x.EmpresaFornecedoraId, x.Codigo}).IsUnique();
        }
    }

    public class ProcuracaoMap : BaseMap<Procuracao>
    {
        public override void Configure(EntityTypeBuilder<Procuracao> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Socio).WithMany(x => x.Procuracoes).HasForeignKey(x => x.SocioId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Outorgante).WithMany().HasForeignKey(x => x.OutorganteId).OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(x => new {x.SocioId, x.OutorganteId}).IsUnique();
        }
    }

    public class AssinaturaSocioMap : BaseMap<AssinaturaSocio>
    {
        public AssinaturaSocioMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }

        public AssinaturaSocioMap() : this(new DefaultMapSettings())
        {
        }

        public override void Configure(EntityTypeBuilder<AssinaturaSocio> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.GrupoDeAssinatura).WithMany(x => x.Assinaturas).HasForeignKey(x => x.GrupoDeAssinaturaId);
            builder.HasOne(x => x.Socio).WithMany().HasForeignKey(x => x.SocioId).OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(x => new {x.GrupoDeAssinaturaId, x.SocioId});
        }
    }

    public class GrupoAssinaturaMap : BaseMap<GrupoDeAssinatura>
    {
        public GrupoAssinaturaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }

        public GrupoAssinaturaMap() : this(new DefaultMapSettings())
        {
        }

        public override void Configure(EntityTypeBuilder<GrupoDeAssinatura> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.EmpresaFornecedora).WithMany(x => x.GruposDeAssinatura).HasForeignKey(x => x.EmpresaFornecedoraId);
        }
    }
}
