using Gicaf.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities.Common
{
    public class Trail
    {
        public TrailOperation Operacao { get; set; }
        public DateTime Data { get; set; }
    }
}
