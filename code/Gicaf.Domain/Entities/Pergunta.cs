using System.Data.SqlTypes;
using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Gicaf.Domain.Validators;

namespace Gicaf.Domain.Entities
{
    public enum Eixo
    {
        X,
        Y
    }

    public enum OrigemDados
    {
        Sistema,
        Formulario
    }

    public class Pergunta : BaseTrailEntity
    {
        public short Codigo { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public Eixo Eixo { get; set; }
        public OrigemDados OrigemDados { get; set; }
        public ICollection<PerguntaGrupoUsuario> PerguntaGrupoUsuario { get; set; }
        public ICollection<Resposta> Respostas { get; set; }
        //public ICollection<Exigencia> Exigencias { get; set; }
    }
}
