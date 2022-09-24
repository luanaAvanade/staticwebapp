using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities.Fornecedores
{
    public class Estado : BaseTrailEntity
    {
        public int CodIBGE { get; set; }
        public string Nome { get; set; }
        public long PaisId { get; set; }
        public Pais Pais { get; set; }
        public string Sigla { get; set;}
        
    }
}
