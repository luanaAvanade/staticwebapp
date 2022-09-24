using Gicaf.Domain.Entities;
using Gicaf.Domain.Interfaces.Repository;
using Gicaf.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Gicaf.Domain.Entities.Fornecedores;

namespace Gicaf.Infra.Data.Repositories
{
    public class EmpresaRepository : RepositoryBase<Empresa>//, IResultadoRepository
    {
        GdriveRepository _gdriveRepository;
        public EmpresaRepository(AppDbContext db, GdriveRepository gdriveRepository) : base(db, gdriveRepository)
        {
            _gdriveRepository = gdriveRepository;
        }
 
        public override Empresa Add(Empresa obj, IDictionary<string, object> input)
        {
            if(obj.Documentos != null)
            {
                foreach(var documento in obj.Documentos)
                {
                    if(documento?.Arquivo?.Conteudo != null)
                    {
                        _gdriveRepository.AnexarDocumentoDaEmpresa(documento);
                    }
                }         
            }
             

            return base.Add(obj, input);
        }
    }
}
