using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Entities.Fornecedores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities
{
    public class ValidadeDocumentoEstado: BaseTrailEntity
    {
        public long EstadoId { get; set; }
        public Estado Estado { get; set; }
        public long TipoDocumentoFuncionalidadeId { get; set; }
        public TipoDocumentoFuncionalidade TipoDocumentoFuncionalidade { get; set; }
        public int? ValidadeMeses { get; set; }
        public bool Obrigatorio {get; set;}
    }
}
