using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class AvaliacaoCategoriaMap : BaseMap<AvaliacaoCategoria>
    {
        public AvaliacaoCategoriaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }

        public AvaliacaoCategoriaMap():this(new DefaultMapSettings())
        {

        }
        public override void Configure(EntityTypeBuilder<AvaliacaoCategoria> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Categoria).WithMany().HasForeignKey(x => x.CategoriaId);
            builder.HasOne(x => x.VersaoMec).WithMany().HasForeignKey(x => x.VersaoMecId);
            builder.HasOne(x => x.Quadrante).WithMany().HasForeignKey(x => x.QuadranteId);
        }
    }
}
