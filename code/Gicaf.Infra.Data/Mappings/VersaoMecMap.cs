using Gicaf.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisAgua.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Mappings
{
    public class VersaoMecMap : BaseMap<VersaoMec>
    {
        public VersaoMecMap(DefaultMapSettings defaultMapSettings) : base(defaultMapSettings)
        {
        }

        public VersaoMecMap() : base(new DefaultMapSettings())
        {

        }

        public override void Configure(EntityTypeBuilder<VersaoMec> builder)
        {
            base.Configure(builder);
        }
    }
}
