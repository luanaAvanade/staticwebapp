using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class EstadoMap : BaseMap<Estado>
    {
        public EstadoMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public EstadoMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<Estado> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Pais).WithMany().HasForeignKey(x => x.PaisId);
        }
    }
}
