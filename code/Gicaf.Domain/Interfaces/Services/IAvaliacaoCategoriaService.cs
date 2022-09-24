using Gicaf.Domain.Entities;
using Gicaf.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Interfaces.Services
{
    public interface IAvaliacaoCategoriaService: IServiceBase<AvaliacaoCategoria>
    {
        void ProcessarMec();
    }
}
