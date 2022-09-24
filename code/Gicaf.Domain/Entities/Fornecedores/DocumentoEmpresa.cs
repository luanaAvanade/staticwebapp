using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities.Fornecedores
{
    //public enum TipoDocumento
    //{
    //    DadosBasicos,
    //    BalancoPatrimonial,
    //    Dre
    //}
    public class DocumentoEmpresa: BaseTrailEntity
    {
        public long EmpresaId { get; set; }
        public Empresa Empresa { get; set; }

        public long ArquivoId { get; set; }
        public Arquivo Arquivo { get; set; }

        //public TipoDocumento TipoDocumento { get; set; }
        public long TipoDocumentoId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }

        public DateTime DataBasePeriodo { get; set; }
    }
}
