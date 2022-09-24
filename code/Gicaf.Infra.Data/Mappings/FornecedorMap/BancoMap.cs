using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class BancoMap : BaseMap<Banco>
    {
        public BancoMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public BancoMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<Banco> builder)
        {
            base.Configure(builder);
        }
    }
}
