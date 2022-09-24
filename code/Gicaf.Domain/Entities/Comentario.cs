using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Entities.Fornecedores;
using Gicaf.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities
{
    public class Comentario : BaseTrailEntity
    {
        public long EmpresaFornecedoraId { get; set; }
        public Empresa Empresa { get; set; }
        public EnumItemAnalise Local { get; set; }
        public long? ArquivoId { get; set; }
        public Arquivo Arquivo { get; set; }

        public long UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
        public string Coment { get; set; }

    }
}
