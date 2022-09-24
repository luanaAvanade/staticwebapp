using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities
{
    public class PerguntaGrupoUsuario : BaseTrailEntity
    {
        public long PerguntaId { get; set; }
        public Pergunta Pergunta { get; set; }
        public long GrupoUsuarioId { get; set; }
        public GrupoUsuario GrupoUsuario { get; set; }
        public TipoItem Tipo { get; set; }
    }
}
