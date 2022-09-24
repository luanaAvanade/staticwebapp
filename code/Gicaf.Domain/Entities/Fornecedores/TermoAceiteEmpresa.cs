using Gicaf.Domain.Entities.Base;
using System;

namespace Gicaf.Domain.Entities.Fornecedores
{
    public class TermoAceiteEmpresa : BaseTrailEntity
    {
         public bool Aceite { get; set; }
         public Empresa Empresa { get; set; }
         public long EmpresaId { get; set; }
         public TermosAceite TermosAceite {get;set;}
         public long TermosAceiteId { get; set; }
         
    }
    
}














