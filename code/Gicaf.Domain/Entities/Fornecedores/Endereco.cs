using System.Data.SqlTypes;
using Gicaf.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Gicaf.Domain.Validators;

namespace Gicaf.Domain.Entities.Fornecedores
{
    public enum TipoLogradouro
    {
        Aeroporto,
        Alameda,
        Area,
        Avenida,
        Campo,
        Chacara,
        Colonia,
        Condominio,
        Conjunto,
        Distrito,
        Esplanada,
        Estacao,
        Estrada,
        Favela,
        Fazenda,
        Feira,
        Jardim,
        Ladeira,
        Lago,
        Lagoa,
        Largo,
        Loteamento,
        Morro,
        Nucleo,
        Parque,
        Passarela,
        Patio,
        Praca,
        Quadra,
        Recanto,
        Residencial,
        Rodovia,
        Rua,
        Setor,
        Sitio,
        Travessa,
        Trecho,
        Trevo,
        Vale,
        Vereda,
        Via,
        Viaduto,
        Viela,
        Vila
    }

    public class Endereco : BaseTrailEntity
    {  
        public string CEP { get; set; }
        public string TipoEndereco { get; set; }
        public string Logradouro { get; set; }
        public TipoLogradouro TipoLogradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public long MunicipioId { get; set; }
        public Municipio Municipio { get; set; }
         public long EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }
}