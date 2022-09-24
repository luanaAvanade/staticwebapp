using Gicaf.Domain.Entities;
using Gicaf.Domain.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Interfaces.Services
{
    public interface IRespostaService: IServiceBase<Resposta>
    {
        IEnumerable<Resposta> GerarRespostas(IQueryNode queryDetails, long perguntaId, long usuarioId);
    }
}
