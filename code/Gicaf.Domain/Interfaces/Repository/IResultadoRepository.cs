using Gicaf.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Interfaces.Repository
{
    public interface IResultadoRepository : IRepositoryBase<Resultado>
    {
        void Processar(long perguntaId);
    }
}
