using Gicaf.Domain.Entities;
using Gicaf.Domain.Interfaces.Repository;
using Gicaf.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Services
{
    public class ResultadoService : ServiceBase<Resultado>, IResultadoService
    {
        public ResultadoService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public void Processar(long perguntaId)
        {
            GetRepository<IResultadoRepository>().Processar(perguntaId);
        }
    }
}
