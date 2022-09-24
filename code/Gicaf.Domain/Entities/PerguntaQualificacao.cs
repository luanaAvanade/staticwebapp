
using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;

namespace Gicaf.Domain.Entities
{
    public enum Perfil
    {
        Fornecedor = 1,
        Analista = 2
    }
    public enum TipoResposta
    {
        NumeroInteiro = 1,
        NumeroDecimal = 2,
        TextoLivre = 3,
        SimNao = 4,
        Lista = 5,
        Data = 6,
        Hora = 7
    }

    public class PerguntaQualificacao : BaseTrailEntity
    {
        public long GrupoPerguntaQualificacaoId { get; set; }
        public GrupoPerguntaQualificacao GrupoPerguntaQualificacao { get; set; }
        public string Texto { get; set; }
        public TipoResposta TipoResposta { get; set; }
        public string ParametroResposta { get; set; }
        public string Dica { get; set; }
        public DateTime? Validade { get; set; }
        public Perfil QuemResponde { get; set; }
        public Perfil? QuemVisualiza { get; set; }
        public bool Obrigatorio { get; set; }
        public bool PossuiAnexo { get; set; }
        public int? TamanhoMaximoArquivo { get; set; }
        public ICollection<PerguntaQualificacaoExigencia> PerguntasQualificacaoExigencia { get; set; }
        public bool Status { get; set; }
  }
}
