using Gicaf.Domain.Entities;
using Gicaf.Domain.Entities.Base;
using Gicaf.Infra.Data.Mappings.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SisAgua.Infra.Mappings
{
    public abstract class BaseMap<TEntity> : DefaultValuesMap<TEntity> where TEntity : BaseEntity
    {
        DefaultMapSettings _defaultMapSettings;

        public BaseMap(DefaultMapSettings defaultMapSettings)
        {
            _defaultMapSettings = defaultMapSettings;
        }

        public BaseMap():this(new DefaultMapSettings())
        {
        }
        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {
            base.Configure(builder);
            builder.HasKey(p => p.Id); 

            if (_defaultMapSettings.ValueGeneratedOnAdd)
            {
                builder.Property(p => p.Id).HasColumnType("bigint").ValueGeneratedOnAdd();
            }
        }
    }
}
