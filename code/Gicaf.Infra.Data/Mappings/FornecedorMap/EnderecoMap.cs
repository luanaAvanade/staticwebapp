using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class EnderecoMap : BaseMap<Endereco>
    {
        public EnderecoMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public EnderecoMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<Endereco> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Empresa).WithMany(x => x.Enderecos).HasForeignKey(x => x.EmpresaId);
            builder.HasOne(x => x.Municipio).WithMany().HasForeignKey(x => x.MunicipioId);
        }
    }
}
