using Gicaf.Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gicaf.Infra.Data.Mappings.Base
{
    public abstract class DefaultValuesMap<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
    {
        DefaultMapSettings _defaultMapSettings;

        public DefaultValuesMap(DefaultMapSettings defaultMapSettings)
        {
            _defaultMapSettings = defaultMapSettings;
        }

        public DefaultValuesMap(): this(new DefaultMapSettings())
        {
        }

        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            foreach (var property in typeof(TEntity).GetProperties().Where(x => x.PropertyType == typeof(string)))
            {
                builder.Property<string>(property.Name).HasColumnType(_defaultMapSettings.DefaultStringDescription).HasMaxLength(_defaultMapSettings.DefaultStringSize);
            }
        }
    }
}
