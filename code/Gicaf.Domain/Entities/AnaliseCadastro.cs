using Gicaf.Domain.Entities.Base;
using Gicaf.Domain.Entities.Fornecedores;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Gicaf.Domain.Entities
{
    public enum StatusAnalise
    {
        [Display(Name="Criado")]
        Criado,
        [Display(Name="Pendente de Análise")]
        Pendente_Analise,
        [Display(Name="Em Análise")]
        Em_Analise,
        [Display(Name="Aprovado")]
        Aprovado,
        [Display(Name="Reprovado")]
        Reprovado,
        [Display(Name="Aprovado com Ressalvas")]
        Aprovado_Ressalvas,
        [Display(Name="Suspenso")]
        Suspenso,
        [Display(Name="Reaberto")]
        Reaberto
    }
    public enum EnumItemAnalise
    {
        [Display(Name="Dados Gerais", GroupName="Dados Básicos")]
        Dados_Gerais,
        [Display(Name="Dados Endereço", GroupName="Dados Básicos")]
        Dados_Endereco,
        [Display(Name="Acesso Sistema", GroupName="Dados Básicos")]
        Acesso_Sistema,
        [Display(Name="Dados Contatos Adicionais", GroupName="Dados Básicos")]
        Dados_Contatos_Adicionais,
        [Display(Name="Dados do Contrato Social", GroupName="Dados Sócios")]
        Dados_Contrato_Social,
        [Display(Name="Cadastro Sócios", GroupName="Dados Sócios")]
        Cadastro_Socios,
        
        [Display(Name="Cadastro de Procuradores", GroupName="Dados Sócios")]
        Cadastro_Procuradores,
        [Display(Name="Cadastro Signatário", GroupName="Dados Sócios")]
        Cadastro_Signatario,
        [Display(Name="Grupos de Fornecimento", GroupName="Dados Complementares")]
        Grupos_Fornecimento,
        [Display(Name="Dados Bancários", GroupName="Dados Complementares")]
        Dados_Bancarios,
        [Display(Name="Dados Balanço Patrimonial", GroupName="Dados Financeiros")]
        Dados_Balanco_Patrimonial,
        [Display(Name="Dados DRE", GroupName="Dados Financeiros")]
        Dados_DRE,
        [Display(Name="Qualificação Financeira", GroupName="Qualificação Financeira")]
        Qualificacao_Risco_Financeiro,
        [Display(Name="Arquivos", GroupName="Arquivos")]
        Arquivo,
        [Display(Name="Dados Pessoa Física", GroupName="Dados Básicos")]
        Dados_Pessoa_Fisica,
        [Display(Name="Contato do Cliente", GroupName="Dados Básicos")]
        Contato_Cliente,
        [Display(Name="Termos de Aceite", GroupName="Dados Básicos")]
        Termos_Aceite
    }
    public class AnaliseCadastro : BaseTrailEntity
    {
        public long EmpresaFornecedoraId { get; set; }
        public Empresa EmpresaFornecedora { get; set; }
        public StatusAnalise StatusAnalise { get; set; }
        public long? AtribuidoId { get; set; }
        public Usuario Atribuido { get; set; }
        public long? TransmitidoId { get; set; }
        public Usuario Transmitido { get; set; }
        public ICollection<ItemAnalise> ItensAnalise { get; set; }
    }

    public class ItemAnalise: BaseTrailEntity{
      public long AnaliseId { get; set; }
      public AnaliseCadastro Analise { get; set; }
      public EnumItemAnalise TipoItem { get; set; }
      public long? AutorId { get; set; }
      public Usuario Autor { get; set; }
      public StatusAnalise Status { get; set; }
      public long? ArquivoId { get; set; }
      public Arquivo Arquivo { get; set; }
      public string Justificativa { get; set; }
    }
}
