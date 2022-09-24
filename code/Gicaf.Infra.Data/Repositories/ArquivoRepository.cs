using Gicaf.Domain.Entities;
using Gicaf.Domain.Interfaces.Repository;
using Gicaf.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gicaf.Infra.Data.Repositories
{
    public class ArquivoRepository: RepositoryBase<Arquivo>, IArquivoRepository
    {
        public ArquivoRepository(AppDbContext db, GdriveRepository gdriveRepository) : base(db, gdriveRepository)
        {
        }

        public override Arquivo Get(long id, object queryDetails)
        {
            var arquivo = base.Get(id, queryDetails);
            if (arquivo.Origem == OrigemArquivo.Gdrive && arquivo.CodigoExterno != null)
            {
                var arquivoGdrive = _gdriveRepository.BuscarConteudoArquivoAsync(arquivo.CodigoExterno).Result;
                arquivo.Conteudo = new List<byte[]> { arquivoGdrive.ConteudoBase64Binary };
            };
            return arquivo;
        }
    }
}
