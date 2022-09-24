using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class TermoAceiteEmpresaMap : BaseMap<TermoAceiteEmpresa>
    {
        public TermoAceiteEmpresaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        
        public TermoAceiteEmpresaMap() : this(new DefaultMapSettings())
        {
        }

        public override void Configure(EntityTypeBuilder<TermoAceiteEmpresa> builder)
        {
            base.Configure(builder);
        }
    }
}
