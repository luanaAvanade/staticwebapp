using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities
{
    public class Resposta : BaseTrailEntity
    {
        public int Nota { get; set; }

        public long PerguntaId { get; set; }
        public Pergunta Pergunta { get; set; }

        public long CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public long UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public VersaoMec VersaoMec { get; set; }
        public long? VersaoMecId { get; set; }
        public bool NaoAplicavel { get; set; }

        public bool Respondida => Nota != 0 || NaoAplicavel;
    }
}
