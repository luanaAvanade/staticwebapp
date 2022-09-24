using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class DadosPessoaFisicaMap : BaseMap<DadosPessoaFisica>
    {
        public DadosPessoaFisicaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public DadosPessoaFisicaMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<DadosPessoaFisica> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Municipio).WithMany().HasForeignKey(x => x.MunicipioId);
        }
    }
}
