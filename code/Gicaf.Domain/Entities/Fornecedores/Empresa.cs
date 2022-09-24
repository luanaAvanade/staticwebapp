

using System;
using System.Collections.Generic;
using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Entities.Enums;

namespace Gicaf.Domain.Entities.Fornecedores
{
    public class Empresa : BaseTrailEntity
    {
        public string NomeEmpresa { get; set; }
        public string CNPJ { get; set; }
        public string InscricaoEstadual { get; set; }
        public bool IsentoIE { get; set; }
        public string InscricaoMunicipal { get; set; }
        public bool OptanteSimplesNacional { get; set; }
        public DateTime DataAbertura { get; set; }
        public string TipoEmpresa { get; set; }
        public string TipoCadastro {get; set; }
        public long? DadosPessoaFisicaId { get; set; }
        public DadosPessoaFisica DadosPessoaFisica { get; set; }
        public ICollection<Contato> ContatosAdicionais { get; set; }
        public ICollection<Usuario> Usuarios { get; set; }
        public Contato ContatoSolicitante {get; set;}
        public long? ContatoSolicitanteId { get; set; }
        public ICollection<Endereco> Enderecos { get; set; } 
        public ICollection<GrupoFornecimento> GruposFornecimento { get; set; } 
        public ICollection<DocumentoEmpresa> Documentos { get; set; }
        public ICollection<DadosBalancoPatrimonial> DadosBalancosPatrimoniais { get; set; }
        public ICollection<DadosDRE> DadosDREs { get; set; }
        public long? DadosBancariosId { get; set; }
        public DadosBancarios DadosBancarios { get; set; }
        public ICollection<Socio> Socios { get; set; }
        public ICollection<GrupoDeAssinatura> GruposDeAssinatura { get; set; }
        public Double? CapitalSocialTotalSociedade { get; set; }
        public DateTime? DataRegistroSociedade { get; set; }
        public AnaliseCadastro AnaliseCadastro { get; set; }
        public long? AtividadeEconomicaPrincipalId { get; set; }
        public AtividadeEconomicaPrincipalCNAE AtividadeEconomicaPrincipal { get; set; }
        public long? OcupacaoPrincipalId { get; set; }
        public OcupacaoPrincipalCNAE OcupacaoPrincipal { get; set; }
        public ICollection<TermoAceiteEmpresa> TermoAceiteEmpresa { get; set; }        
        public List<CalculoRisco> CalculoRiscoLista { get; set; }
        public string CodigoSap { get; set; }
        public SituacaoFornecedor Situacao { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public string LinkCadastro { get; set; }
    }
}