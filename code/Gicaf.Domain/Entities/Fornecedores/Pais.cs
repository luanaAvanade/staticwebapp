using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities.Fornecedores
{
    public class Pais : BaseTrailEntity
    {
        public int CodBACEN { get; set; }
        public string Nome { get; set; }
    }
}
