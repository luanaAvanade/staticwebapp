using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class QuadranteMap : BaseMap<Quadrante>
    {
        public QuadranteMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public QuadranteMap() : this(new DefaultMapSettings())
        {
        }

        public override void Configure(EntityTypeBuilder<Quadrante> builder)
        {
            base.Configure(builder);
            builder.Property(x => x.Id).ValueGeneratedNever();
        }
    }
}
