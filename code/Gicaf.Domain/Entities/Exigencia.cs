using System;
using System.Collections.Generic;
using Gicaf.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Gicaf.Domain.Entities
{
    public class Exigencia : BaseTrailEntity
    {
        public string Nome { get; set; }

        public long TipoExigenciaId { get; set; }
        public TipoExigencia TipoExigencia { get; set; }
        public ICollection<ExigenciaGrupoQualificacao> ExigenciasGrupoQualificacao { get; set; }
        public ICollection<PerguntaQualificacaoExigencia> PerguntasQualificacaoExigencia { get; set; }
        public bool Status { get; set; }
    }
}
