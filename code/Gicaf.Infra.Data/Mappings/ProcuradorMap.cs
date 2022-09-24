using Gicaf.Domain.Entities;
using Gicaf.Infra.Data.Mappings.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    // public class ProcuradorMap : DefaultValuesMap<Procurador>
    // {
    //     public override void Configure(EntityTypeBuilder<Procurador> builder)
    //     {
    //         base.Configure(builder);
            //builder.HasOne(x => x.Representado).WithMany().HasForeignKey(x => x.RepresentadoId).OnDelete(DeleteBehavior.Restrict);
            //builder.Property<PapelPessoa>("Papel").HasDefaultValue(PapelPessoa.Procurador);
            //builder.HasQueryFilter(x => EF.Property<PapelPessoa>(x, "Papel").HasFlag(PapelPessoa.Procurador));
    //     }
    // }
}
