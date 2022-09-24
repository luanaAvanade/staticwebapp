using System;
using System.Collections.Generic;
using Gicaf.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Gicaf.Domain.Entities
{
    [Flags]
    public enum TiposArq
    {
        PDF = 1,
        XML = 2,
        CSV = 4,
        JPG = 8
    }

    public class TipoDocumento : BaseTrailEntity
    {
        public string Nome { get; set; }
        public string Help { get; set; }
        public int? QuantidadeMaxima { get; set; }
        public int? TamanhoMaximo { get; set; }
        public bool Obrigatorio { get; set; }
        public int? ValidadeMeses { get; set; }
        public bool Status { get; set; }
        public TiposArq TiposArquivos { get; set; }
        //public Funcionalidades Funcionalidades { get; set; }
        //public ICollection<ValidadeDocumentoEstado> ValidadeDocumentoEstado { get; set; }
        public ICollection<TipoDocumentoFuncionalidade> TipoDocumentoFuncionalidade { get; set; }
    }
}