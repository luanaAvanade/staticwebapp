using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    [Flags]
    public enum PapelPessoa: int
    {
        Pessoa = 1,
        Socio = 2,
        Procurador = 3
    }

    public class PessoaMap : BaseMap<Pessoa>
    {
        public PessoaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }

        public PessoaMap() : this(new DefaultMapSettings())
        {
        }

        public override void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            base.Configure(builder);
            
            builder.HasDiscriminator<PapelPessoa>("Papel")
                .HasValue<Pessoa>(PapelPessoa.Pessoa);

            builder.HasIndex(nameof(Pessoa.CPF), "Papel").IsUnique();
        }
    }
}
