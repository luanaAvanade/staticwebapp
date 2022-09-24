using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class FormulaMap : BaseMap<Formula>
    {
        public FormulaMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }
        // off
        //public FormulaMap() : this(new DefaultMapSettings())
        //{
        //}
        public override void Configure(EntityTypeBuilder<Formula> builder)
        {
            base.Configure(builder);
        }
    }
}
