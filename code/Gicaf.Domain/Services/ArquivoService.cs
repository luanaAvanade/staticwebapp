using Gicaf.Domain.Entities;
using Gicaf.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
//using Gicaf.

namespace Gicaf.Domain.Services
{
    public class ArquivoService : ServiceBase<Arquivo>, IArquivoService
    {
        public ArquivoService(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
