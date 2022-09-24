using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities
{
    public class Resultado: BaseTrailEntity
    {
        public double Nota { get; set; }
        public double? Moda { get; set; }
        public double? Media { get; set; }
        public double? Mediana { get; set; }
        public long PerguntaId { get; set; }
        public Pergunta Pergunta { get; set; }
        public long CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        public long? AvaliacaoCategoriaId { get; set; }
        public AvaliacaoCategoria AvaliacaoCategoria { get; set; }
        public long? ArquivoProcessamentoPerguntaId { get; set; }
        public ArquivoProcessamentoPergunta ArquivoProcessamentoPergunta { get; set; }
        public VersaoMec VersaoMec { get; set; }
        public long? VersaoMecId { get; set; }
    }
}
