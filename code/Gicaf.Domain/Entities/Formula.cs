using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities
{
    public class Formula : BaseTrailEntity
    {
        public string Descricao { get; set; }
        public bool Selecionavel { get; set; }
    }
}
