using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Gicaf.Domain.Entities
{
    public class Coordenada : BaseTrailEntity
    {
        public float Altitude { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }

        public IEnumerable<string> ValidationErrors()
        {
            var erros = new CoordenadaValidator().Validate(this).Errors;
            //erros.Select(x => x.prop)
            return erros.Select(x => x.ErrorMessage);
        }
    }
}
