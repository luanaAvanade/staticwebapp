using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Entities.Fornecedores;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Domain.Entities
{
    public enum TipoPessoa
    {
        PessoaFisica,
        PessoaJuridica
    }
    public enum TipoSocio
    {
        Socio,
        Procurador,
        Socio_Procurador,
    }
    public class Socio : BaseTrailEntity
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public long EmpresaFornecedoraId { get; set; }
        public Empresa EmpresaFornecedora { get; set; }
        public TipoPessoa TipoPessoa { get; set; }
        public TipoSocio TipoSocio { get; set; }
        public double? ValorParticipacao { get; set; }
        public bool? Administrador { get; set; }
        public ICollection<Procuracao> Procuracoes { get; set; }
    }

    public class Procuracao: BaseTrailEntity{
        public long SocioId { get; set; }
        public Socio Socio { get; set;}
        public long OutorganteId { get; set; }
        public Socio Outorgante { get; set; }
        public DateTime Validade { get; set; }
    }
    public enum TipoAssinatura
    {
        Individual,
        Conjunto
    }
    public class GrupoDeAssinatura: BaseTrailEntity
    {
        public long EmpresaFornecedoraId { get; set; }
        public Empresa EmpresaFornecedora { get; set; }
        public TipoAssinatura TipoAssinatura { get; set; }
        public double ValorLimite { get; set; }
        public ICollection<AssinaturaSocio> Assinaturas { get; set; }
    }
    public class AssinaturaSocio: BaseTrailEntity
    {
        public bool Obrigatoriedade { get; set; }
        public long GrupoDeAssinaturaId { get; set; }
        public GrupoDeAssinatura GrupoDeAssinatura { get; set; }
        public long SocioId { get; set; }
        public Socio Socio { get; set; }
    }
}
