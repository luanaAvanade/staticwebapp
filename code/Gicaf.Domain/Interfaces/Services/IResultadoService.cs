using Gicaf.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Interfaces.Services
{
    public interface IResultadoService: IServiceBase<Resultado>
    {
        void Processar(long perguntaId);
    }
}
