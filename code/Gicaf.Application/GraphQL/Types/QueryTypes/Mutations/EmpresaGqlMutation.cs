using Gicaf.Application.GraphQL.Types.Base;
using Gicaf.Application.GraphQL.Types.InputTypes;
using Gicaf.Domain.Entities.Fornecedores;
using Gicaf.Domain.Interfaces.Services;
using Gicaf.Domain.Services;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Application.GraphQL.Types.QueryTypes.Mutations
{
    public class EmpresaCreateInput : BaseInputType<Empresa>
    {
        public EmpresaCreateInput()
        {
            Field(x => x.CNPJ);
            Field(x => x.NomeEmpresa);
            Field(x => x.InscricaoEstadual);
            Field(x => x.InscricaoMunicipal);
            Field(x => x.IsentoIE);
            Field(x => x.OptanteSimplesNacional);
            Field(x => x.DataAbertura);
            Field(x => x.TipoEmpresa);
            Field(x => x.TipoCadastro);
            Field(x => x.Enderecos, false, typeof(ListGraphType<EnderecoCreateInput>));
            Field(x => x.ContatoSolicitante, false, typeof(ContatoCreateInput));
            Field(x => x.ContatosAdicionais, false, typeof(ListGraphType<ContatoCreateInput>));
            Field(x => x.Usuarios, false, typeof(ListGraphType<UsuarioCreateInput>));
            Field(x => x.AtividadeEconomicaPrincipalId);
            Field(x => x.OcupacaoPrincipalId);
            Field(x => x.DadosPessoaFisica, true, typeof(DadosPessoaFisicaCreateInput));
        }
    }

    public class EmpresaUpdateInput : BaseInputType<Empresa>
    {
        public EmpresaUpdateInput()
        {
            Field(x => x.CNPJ, true);
            Field(x => x.NomeEmpresa, true);
            Field(x => x.InscricaoEstadual, true);
            Field(x => x.InscricaoMunicipal, true);
            Field(x => x.IsentoIE, true);
            Field(x => x.OptanteSimplesNacional, true);
            Field(x => x.DataAbertura, true);
            Field(x => x.TipoEmpresa, true);
            Field(x => x.TipoCadastro, true);
            Field(x => x.Enderecos, false, typeof(ListGraphType<EnderecoCreateInput>));
            Field(x => x.ContatoSolicitante, false, typeof(ContatoCreateInput));
            Field(x => x.ContatosAdicionais, false, typeof(ListGraphType<ContatoCreateInput>));
            Field(x => x.Usuarios, false, typeof(ListGraphType<UsuarioCreateInput>));
            Field(x => x.AtividadeEconomicaPrincipalId, true);
            Field (x => x.OcupacaoPrincipalId,true);
            Field(x => x.DadosPessoaFisica, true, typeof(DadosPessoaFisicaCreateInput));
            Field(x => x.DadosBancarios, true, typeof(DadosBancariosInput));
            Field(x => x.GruposFornecimento, true, typeof(ListGraphType<GrupoFornecimentoInput>));
        }
    }

    public class EmpresaGqlMutation : GqlMutationFieldsBase<EmpresaService>
    {
        public EmpresaGqlMutation(EntityType entityType)
            : base(entityType, new EmpresaCreateInput(), new EmpresaUpdateInput())
        {
        }
    }
}
