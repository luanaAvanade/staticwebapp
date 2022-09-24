using Gicaf.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Interfaces.Repository
{
    public interface IAvaliacaoCategoriaRepository: IRepositoryBase<AvaliacaoCategoria>
    {
        void ProcessarMec(long formulaId);
    }
}
