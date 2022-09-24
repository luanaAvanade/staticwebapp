using Gicaf.Domain.Entities;
using Gicaf.Domain.Interfaces.Repository;
using Gicaf.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gicaf.Domain.Services
{
    public class AvaliacaoCategoriaService : ServiceBase<AvaliacaoCategoria>, IAvaliacaoCategoriaService
    {
        public AvaliacaoCategoriaService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public void ProcessarMec()
        {
            var versaoId = GetRepository<IServiceBase<VersaoMec>>().GetWhere(null, x => !x.Encerrado).FirstOrDefault().Id;
            GetRepository<IAvaliacaoCategoriaRepository>().ProcessarMec(versaoId);
            //throw new NotImplementedException();
        }
    }
}
