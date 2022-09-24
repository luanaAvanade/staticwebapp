using System.Data.SqlTypes;
using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Gicaf.Domain.Validators;

namespace Gicaf.Domain.Entities.Fornecedores
{
    public class Contato : BaseTrailEntity
    {  
        public string NomeContato { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public long? TipoContatoId { get; set; }
        public TipoContato TipoContato { get; set; }
        public Empresa Empresa { get; set; }
        public long? EmpresaId { get; set; }
    }
}