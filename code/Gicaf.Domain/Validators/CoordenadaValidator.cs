using Gicaf.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using FluentValidation;

namespace Gicaf.Domain.Validators
{
    public class CoordenadaValidator: BaseValidator<Coordenada>
    {
        const int MIN_LATITUDE = -90;
        const int MAX_LATITUDE = 90;
        
        const int MIN_LONGITUDE = -180;
        const int MAX_LONGITUDE = 180;

        //public CoordenadaValidator(bool addDefaultRules = true) : base(addDefaultRules)
        public CoordenadaValidator(params string[] contexts) : base(contexts)
        {
            ValidarIntervaloDeValores();
        }

        //public override void DefaultRules()
        //{
        //    ValidarIntervaloDeValores();
        //}

        public void ValidarIntervaloDeValores()
        {
            RuleDisplayFor(p => p.Latitude).InclusiveBetween(MIN_LATITUDE, MAX_LATITUDE)
                .WithMessageFromResource(DEFAULT_RANGE_ERROR_MSG);

            RuleDisplayFor(p => p.Longitude).InclusiveBetween(MIN_LONGITUDE, MAX_LONGITUDE)
                .WithMessageFromResource(DEFAULT_RANGE_ERROR_MSG);
        }
    }
}
