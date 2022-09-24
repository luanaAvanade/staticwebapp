using Gicaf.Domain.Entities;
using Gicaf.Domain.Entities.Fornecedores;
using Gicaf.Infra.Data.Context;
using Gicaf.Infra.Data.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gicaf.Infra.Data.Repositories
{
    public class DocumentoEmpresaRepository : RepositoryBase<DocumentoEmpresa>
    {
        public DocumentoEmpresaRepository(AppDbContext db, GdriveRepository gdriveRepository) : base(db, gdriveRepository)
        {
            _gdriveRepository = gdriveRepository;
        }

        public override DocumentoEmpresa Get(long id, object queryDetails)
        {
            var documento = base.Get(id, queryDetails);

            _dbSet.Where(x => x.Id == id).Include(x => x.Arquivo).FirstOrDefault();

            if(documento.Arquivo != null)
            {
                var conteudo = _gdriveRepository.BuscarConteudoArquivoAsync(documento.Arquivo.CodigoExterno).Result.ConteudoBase64Binary;
                var c = documento.Arquivo.Conteudo = new List<byte[]> { conteudo };
            }
            return documento;
        }


        public override DocumentoEmpresa Add(DocumentoEmpresa documentoEmpresa, IDictionary<string, object> input)
        {
            documentoEmpresa.Arquivo.Origem = OrigemArquivo.Gdrive;
            _gdriveRepository.AnexarDocumentoDaEmpresa(documentoEmpresa);
            var entry = base.Add(documentoEmpresa, input);
            return entry;
        }
    }
}
