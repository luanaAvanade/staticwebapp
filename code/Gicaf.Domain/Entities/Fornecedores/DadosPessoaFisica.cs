using System.Data.SqlTypes;
using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Gicaf.Domain.Validators;

namespace Gicaf.Domain.Entities.Fornecedores
{

    public enum Sexo{
        M=1, 
        F=2
    }

    public class DadosPessoaFisica : BaseTrailEntity
    {
        public string CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public string PisPasepNit { get; set; }
        public long MunicipioId { get; set; }
        public Municipio Municipio { get; set; }
        public Sexo Sexo { get; set; }
    }
}