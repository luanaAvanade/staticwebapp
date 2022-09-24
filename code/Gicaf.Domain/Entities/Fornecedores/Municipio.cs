using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities.Fornecedores
{
    public class Municipio : BaseTrailEntity
    {
        public int CodIBGE { get; set; }
        public string Nome { get; set; }
        public long EstadoId { get; set; }
        public Estado Estado { get; set; }
    }
}
