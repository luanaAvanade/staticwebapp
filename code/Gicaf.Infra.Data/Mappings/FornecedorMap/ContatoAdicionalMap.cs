using Gicaf.Domain.Entities.Fornecedores;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class ContatoMap : BaseMap<Contato>
    {
        public ContatoMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        public ContatoMap() : this(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<Contato> builder)
        {
            base.Configure(builder);
            builder.HasOne(x => x.Empresa).WithMany(x => x.ContatosAdicionais).HasForeignKey(x => x.EmpresaId);
            }
    }
}
