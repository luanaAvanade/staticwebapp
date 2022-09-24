using System;
using System.Collections.Generic;
using Gicaf.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Gicaf.Domain.Entities
{
    public class PerguntaQualificacaoExigencia : BaseTrailEntity
    {
        public long PerguntaQualificacaoId { get; set; }
        public PerguntaQualificacao PerguntaQualificacao { get; set; }
        public long ExigenciaId { get; set; }
        public Exigencia Exigencia { get; set; }
    }
}