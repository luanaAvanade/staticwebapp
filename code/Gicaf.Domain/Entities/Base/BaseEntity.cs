using Gicaf.Domain.Validators;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities.Base
{
    public abstract class BaseEntity//<TId>
    {
        public long Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataModificacao { get; set; }

        //public abstract bool IsValid();

        //public ValidationResult Validate()
        //{
        //    return null;
        //    //return new ValidationResult();
        //    //return new CoordenadaValidator().Validate(default(Coordenada));
        //}
    }
}
