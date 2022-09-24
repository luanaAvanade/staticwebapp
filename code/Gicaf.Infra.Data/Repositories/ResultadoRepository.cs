using Gicaf.Domain.Entities;
using Gicaf.Domain.Interfaces.Repository;
using Gicaf.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Repositories
{
    public static class SSISConfig
    {
        public const string SP_PROCESSAR_PERGUNTA_COD_PERGUNTA = @"dbo.sp_ProcessarPergunta @p_codigo_pergunta = {0}, @p_job_name = {1}";
        public const string SP_PROCESSAR_PERGUNTA = @"dbo.sp_ProcessarPergunta @p_arquivo_proc_pergunta_id = {0}, @p_job_name = {1}";

        public static string VolumeCompras;
        public static string PrazoMedioSubstituicaoFornecedor;
        public static string NumeroFornecedores;
        public static string GrauRegulamentacao;
        public static string PerguntaFormulario;

        public static string RetornaNomeJob(long perguntaId)
        {
            switch (perguntaId)
            {
                case (1): return VolumeCompras;
                case (4): return PrazoMedioSubstituicaoFornecedor;
                case (5): return NumeroFornecedores;
                case (6): return GrauRegulamentacao;
                default: return PerguntaFormulario;
            }
        }
    }

    public class ResultadoRepository : RepositoryBase<Resultado>, IResultadoRepository
    {
        public ResultadoRepository(AppDbContext db, GdriveRepository gdriveRepository) : base(db, gdriveRepository)
        {
        }

        public void Processar(long perguntaId)
        {
            string nomeJob = SSISConfig.RetornaNomeJob(perguntaId);
            _db.Database.ExecuteSqlCommand(SSISConfig.SP_PROCESSAR_PERGUNTA_COD_PERGUNTA, perguntaId, nomeJob);
        }
    }
}
