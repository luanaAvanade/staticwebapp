using Gicaf.Domain.Entities;
using Gicaf.Domain.Interfaces.Repository;
using Gicaf.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace Gicaf.Infra.Data.Repositories
{
    public class ArquivoProcessamentoPerguntaRepository : RepositoryBase<ArquivoProcessamentoPergunta>, IArquivoProcessamentoPerguntaRepository
    {
        public ArquivoProcessamentoPerguntaRepository(AppDbContext db, GdriveRepository gdriveRepository) : base(db, gdriveRepository)
        {
        }

        public override ArquivoProcessamentoPergunta Add(ArquivoProcessamentoPergunta arquivoProcessamentoPergunta, IDictionary<string, object> input)
        {
            var arquivo = arquivoProcessamentoPergunta.Arquivo;
            arquivo.Caminho = String.Join(";",arquivo.Conteudo.Select(x => Config.DirArquivoImportacao));

            for (int i = 0; i < arquivo.CaminhoCompleto.Count() ; i++)
            {
                File.WriteAllBytes(arquivo.CaminhoCompleto.ElementAt(i), arquivo.Conteudo.ElementAt(i));
            }
            base.Add(arquivoProcessamentoPergunta, input);
            _db.SaveChanges();
            _db.Database.ExecuteSqlCommand(SSISConfig.SP_PROCESSAR_PERGUNTA, arquivoProcessamentoPergunta.Id, SSISConfig.RetornaNomeJob(arquivoProcessamentoPergunta.PerguntaId));

            var entity = _db.ChangeTracker.Entries().FirstOrDefault(x => x.Entity.GetType().Name == nameof(ArquivoProcessamentoPergunta));
            entity.Reload();

            var arq = Get(arquivoProcessamentoPergunta.Id, null);
            return arq;
        }

        public override ArquivoProcessamentoPergunta Update(ArquivoProcessamentoPergunta arquivoProcessamentoPergunta, IDictionary<string, object> inputs)
        {
            var arquivo = arquivoProcessamentoPergunta.Arquivo;

            for (int i = 0; i < arquivo.CaminhoCompleto.Count(); i++)
            {
                File.WriteAllBytes(arquivo.CaminhoCompleto.ElementAt(i), arquivo.Conteudo.ElementAt(i));
            }

            return base.Update(arquivoProcessamentoPergunta, inputs);
        }
    }
}
