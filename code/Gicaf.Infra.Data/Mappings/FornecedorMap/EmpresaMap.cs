using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;

namespace Gicaf.Infra.Data.Mappings
{
    public class EmpresaMap : BaseMap<Empresa>
    {
        public EmpresaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public EmpresaMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<Empresa> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.DadosPessoaFisica).WithMany().HasForeignKey(x => x.DadosPessoaFisicaId);
            builder.HasOne(x => x.ContatoSolicitante).WithMany().HasForeignKey(x => x.ContatoSolicitanteId);        
            builder.HasOne(x => x.ContatoSolicitante).WithMany().HasForeignKey(x => x.ContatoSolicitanteId);
            builder.HasOne(x => x.DadosBancarios).WithMany().HasForeignKey(x => x.DadosBancariosId);
            builder.HasOne(x => x.AtividadeEconomicaPrincipal).WithMany().HasForeignKey(x => x.AtividadeEconomicaPrincipalId);
            builder.HasOne(x => x.OcupacaoPrincipal).WithMany().HasForeignKey(x => x.OcupacaoPrincipalId);
            builder.Ignore(x => x.LinkCadastro);
        }
    }
}

