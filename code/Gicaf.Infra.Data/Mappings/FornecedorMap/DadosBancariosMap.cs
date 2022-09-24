using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings.FornecedorMap
{
    public class DadosBancariosMap : BaseMap<DadosBancarios>
    {
        public DadosBancariosMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public DadosBancariosMap() : this(new DefaultMapSettings())
        {
        }

        public override void Configure(EntityTypeBuilder<DadosBancarios> builder) {
             base.Configure(builder);
        }
    }
}