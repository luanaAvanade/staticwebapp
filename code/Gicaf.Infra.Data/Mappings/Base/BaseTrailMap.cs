using Gicaf.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings.Base
{
    public abstract class BaseTrailMap<TEntity> : BaseMap<TEntity> where TEntity : BaseTrailEntity
    {
        DefaultMapSettings _defaultMapSettings;

        public BaseTrailMap(DefaultMapSettings defaultMapSettings):base(defaultMapSettings)
        {
            _defaultMapSettings = defaultMapSettings;
        }

        public BaseTrailMap():this(new DefaultMapSettings())
        {
        }

        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);
            //builder.OwnsOne(x => x.Trail, trail => { trail.Property(p => p.Data).HasColumnName("Data"); trail.Property(p => p.Operacao).HasColumnName("Operacao"); });
            builder.Ignore(x => x.DataCriacao);
            builder.Ignore(x => x.DataModificacao);
        }
    }
}
