using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities
{
    public class ParametroPergunta: BaseTrailEntity
    {
        public short Codigo { get; set; }
        public long PerguntaId { get; set; }
        public Pergunta Pergunta { get; set; }
        public string Nome { get; set; }
        public float Peso { get; set; }
    }
}
