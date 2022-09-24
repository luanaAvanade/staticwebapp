using Gicaf.Domain.Entities.Base;
using System.Collections.Generic;
using System;

namespace Gicaf.Domain.Entities.Fornecedores
{
    public class TermosAceite : BaseTrailEntity
    {
        public string Titulo  { get; set; }
        public string SubTitulo  { get; set; }
        public bool Status { get; set; }
         public string TipoFornecedor  { get; set; }
         public string TipoCadastro  { get; set; }
         public string Descricao  { get; set; }
         public ICollection<TermoAceiteEmpresa> TermoAceiteEmpresa { get; set; }

    }
}