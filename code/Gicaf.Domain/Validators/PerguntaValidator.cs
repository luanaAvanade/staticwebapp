using Gicaf.Domain.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Validators
{
    public class PerguntaValidator : BaseValidator<Pergunta>
    {
        public PerguntaValidator(params string[] contexts) : base(contexts)
        {
            RuleDisplayFor(p => p.Nome).NotEmpty();
            RuleDisplayFor(p => p.Descricao).NotEmpty();
        }

        public PerguntaValidator():this(null)
        {
        }
    }
}
