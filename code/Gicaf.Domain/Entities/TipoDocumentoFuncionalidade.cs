using System;
using System.Collections.Generic;
using Gicaf.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Gicaf.Domain.Entities
{
    public enum Funcionalidades
    {
            Cadastro_MEI = 1,
        Cadastro_Pessoa_Juridica = 2,
        Cadastro_Empresa_Estrangeira = 3,
        Cadastro_Socio_MEI = 4,
        Cadastro_Socio_Pessoa_Juridica = 5,
        Cadastro_Socio_Empresa_Estrangeira = 6,
        Cadastro_Financeiro_MEI = 7,
        Cadastro_Financeiro_Pessoa_Juridica = 8,
        Cadastro_Financeiro_Pessoa_Estrangeira = 9,
        Cadastro_MEI_Descentralizado = 10,
        Cadastro_Pessoa_Juridica_Descentralizado = 11,
        Cadastro_Empresa_Estrangeira_Descentralizado = 12,
    }

    public class TipoDocumentoFuncionalidade : BaseTrailEntity
    {
        public long TipoDocumentoId { get; set; }
        public TipoDocumento TipoDocumento { get; set; }
        public Funcionalidades Funcionalidade { get; set; }
        public bool Obrigatorio { get; set; }
        public int? ValidadeMeses { get; set; }
        public ICollection<ValidadeDocumentoEstado> ValidadeDocumentoEstado { get; set; }

        public TipoDocumentoFuncionalidade(){
            ValidadeDocumentoEstado = new List<ValidadeDocumentoEstado>();
        }
    }

    
}