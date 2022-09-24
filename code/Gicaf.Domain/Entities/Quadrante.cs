using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities
{
    public class Quadrante: BaseTrailEntity
    {
        public string Descricao { get; set; }
        public NivelRisco RiscoSegurancaSaude { get; set; }
        public NivelRisco RiscoAmbiental { get; set; }
        public NivelRisco RiscoResponsabilidadeSocial { get; set; }
        public NivelRisco RiscoIntegridade { get; set; }
    }
}
