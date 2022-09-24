using Gicaf.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Interfaces.Repository
{
    public interface IArquivoRepository: IRepositoryBase<Arquivo>
    {
    }


    public interface IArquivoProcessamentoPerguntaRepository: IRepositoryBase<ArquivoProcessamentoPergunta>
    {
    }
}
