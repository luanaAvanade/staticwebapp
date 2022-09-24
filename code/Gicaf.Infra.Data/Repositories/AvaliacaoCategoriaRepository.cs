using Gicaf.Domain.Entities;
using Gicaf.Domain.Interfaces.Repository;
using Gicaf.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Repositories
{
    public class AvaliacaoCategoriaRepository : RepositoryBase<AvaliacaoCategoria>, IAvaliacaoCategoriaRepository
    {
        const string CALCULAR_MATRIZ_SP = @"dbo.sp_CalcularMatriz @versaoId  = {0}";

        public AvaliacaoCategoriaRepository(AppDbContext db, GdriveRepository gdriveRepository) : base(db, gdriveRepository)
        {
        }

        public void ProcessarMec(long formulaId)
        {
            _db.Database.ExecuteSqlCommand(CALCULAR_MATRIZ_SP, formulaId);
        }
    }
}
