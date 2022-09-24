using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities
{
    public class Categoria : BaseTrailEntity
    {
        public string Codigo { get; set; }
        //public string Nome { get; set; }
        public string Descricao { get; set; }
        public ICollection<GrupoCategoria> Grupos { get; set; }
        public TipoItem? Tipo { get; set; }

        public long? VersaoMecId { get; set; }
        public VersaoMec VersaoMec { get; set; }

        //public ICollection<GrupoCategoria> GruposCategoria { get; set; }
    }
}
