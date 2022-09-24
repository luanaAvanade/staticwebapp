using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class MunicipioMap : BaseMap<Municipio>
    {
        public MunicipioMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public MunicipioMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<Municipio> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Estado).WithMany().HasForeignKey(x => x.EstadoId);
        }
    }
}
