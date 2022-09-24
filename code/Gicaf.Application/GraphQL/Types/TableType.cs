using Gicaf.Application.GraphQL.Models;
using Gicaf.Application.GraphQL.Resolvers;
using Gicaf.Application.GraphQL.Types.InputTypes;
using Gicaf.Domain.Entities;
using Gicaf.Domain.Entities.Fornecedores;
using GraphQL;
using GraphQL.Resolvers;
using GraphQL.Types;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gicaf.Application.GraphQL.Types
{
    public class EntityType : ObjectGraphType<object>
    {
        public static QueryArguments DefaultListArgs =>
        new QueryArguments(
            new QueryArgument<StringGraphType> { Name = Constants.Arguments.alias },
            new QueryArgument<IntGraphType> { Name = Constants.Arguments.take},
            new QueryArgument<IntGraphType> { Name = Constants.Arguments.skip},
            new QueryArgument<StringGraphType> { Name = Constants.Arguments.where },
            new QueryArgument<StringGraphType> { Name = Constants.Arguments.orderBy },
            new QueryArgument<BooleanGraphType> { Name = Constants.Arguments.paged }
        );

        public EntityMetadata EntityMetadata { get; set; }
        public string _friendlyTableName { get; set; }
        public EntityType(EntityMetadata tableMetadata, string friendlyTableName)
        {
            EntityMetadata = tableMetadata;
            _friendlyTableName = friendlyTableName;
            Name = tableMetadata.Name;

            foreach (var tableColumn in tableMetadata.Columns.Where(x => x.IsScalar))
            {
                InitGraphTableColumn(tableColumn);
            }
        }

        public QueryArguments TableArgs
        {
            get; set;
        }

        private IDictionary<string, Type> _databaseTypeToSystemType;
        protected IDictionary<string, Type> DatabaseTypeToSystemType
        {
            get
            {
                if (_databaseTypeToSystemType == null)
                {
                    _databaseTypeToSystemType = new Dictionary<string, Type> {
                        { "uniqueidentifier", typeof(String) },
                        { "char", typeof(String) },
                        { "nvarchar", typeof(String) },
                        { "int", typeof(int) },
                        { "decimal", typeof(decimal) },
                        { "bit", typeof(bool) }
                    };
                }
                return _databaseTypeToSystemType;
            }
        }

        private void InitGraphTableColumn(ColumnMetadata columnMetadata)
        {
            var columnField = Field((ResolveColumnMetaType(columnMetadata)),columnMetadata.ColumnName);
            columnField.ResolvedType = new StringGraphType();
            columnField.Resolver = new NameFieldResolver();
        }

        private Type ResolveColumnMetaType(ColumnMetadata columnMetadata)
        {
            if (columnMetadata.Type.IsEnum)
            {
                var enumType = typeof(EnumerationGraphType<>);
                enumType = enumType.MakeGenericType(columnMetadata.Type);

                //if (columnMetadata.Type.GetCustomAttributes(typeof(FlagsAttribute), true).Any())
                //{
                //    enumType = typeof(ListGraphType<>).MakeGenericType(enumType);
                //}
                return enumType;
            }

            if (DatabaseTypeToSystemType.ContainsKey(columnMetadata.DataType))
                return DatabaseTypeToSystemType[columnMetadata.DataType].GetGraphTypeFromType(true);

            return typeof(string).GetGraphTypeFromType(true);
        }

        public static QueryArguments GetArgumentsFor(string name, Resolvers.ResolverType resolverType)
        {
            QueryArguments arguments = new QueryArguments();
            //QueryArgument argument = null;

            switch (resolverType)
            {
                case Resolvers.ResolverType.Insert:
                {
                    var input = GetCreateInputArgument(name);
                    if (input != null)
                    {
                        arguments.Add(input);
                    }

                    break;
                }

                case Resolvers.ResolverType.InsertMany:
                {
                    var input = GetCreateInputArgument(name, true);
                    if (input != null)
                    {
                        arguments.Add(input);
                    }

                    break;
                }

                case Resolvers.ResolverType.Update:
                {
                    var input = GetUpdateInputArgument(name);
                    if (input != null)
                    {
                        arguments.Add(input);
                    }

                    break;
                }
                case Resolvers.ResolverType.UpdateMany:
                {
                    var input = GetUpdateInputArgument(name, true);
                    if (input != null)
                    {
                        arguments.Add(input);
                    }

                    break;
                }
                case Resolvers.ResolverType.RemoveMany:
                {
                    var input = GetUpdateInputArgument(name, true);
                    if (input != null)
                    {
                        arguments.Add(input);
                    }

                    break;
                }
                case Resolvers.ResolverType.CountBy:
                {
                    arguments.Add(new QueryArgument<StringGraphType>() { Name = Constants.Arguments.where });
                    arguments.Add(new QueryArgument<StringGraphType>() { Name = Constants.Arguments.groupBy });
                    break;
                }
                case Resolvers.ResolverType.List:
                    arguments = DefaultListArgs;
                    break;
                case Resolvers.ResolverType.ProcessarResultado:
                {
                    arguments.Add(new QueryArgument<IntGraphType>() { Name = Constants.Arguments.codigoPergunta });
                    break;
                }
                case Resolvers.ResolverType.GerarRespostas:
                {
                    arguments.Add(InputArgumentFor<GerarRespostasInput>(false));
                    break;
                }
                case Resolvers.ResolverType.Aggregate:
                {
                    arguments.Add(new QueryArgument<ListGraphType<StringGraphType>>() { Name = Constants.Arguments.functions });
                    arguments.Add(new QueryArgument<StringGraphType>() { Name = Constants.Arguments.where });
                    arguments.Add(new QueryArgument<StringGraphType>() { Name = Constants.Arguments.groupBy });
                    break;
                }
            }

            if (resolverType == Resolvers.ResolverType.FindById ||
                resolverType == Resolvers.ResolverType.Remove ||
                resolverType == Resolvers.ResolverType.Update)
            {
                arguments.Add(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = Constants.Arguments.id });
            }
            else if (resolverType == Resolvers.ResolverType.UpdateMany || resolverType == Resolvers.ResolverType.RemoveMany)
            {
                arguments.Add(new QueryArgument<ListGraphType<NonNullGraphType<IdGraphType>>> { Name = Constants.Arguments.ids });
            }

            return arguments;
        }

        private static QueryArgument GetUpdateInputArgument(string name, bool isListType = false)
        {
            switch (name)
            {
                case nameof(ArquivoProcessamentoPergunta):
                    return InputArgumentFor<ArquivoProcessamentoPerguntaInput>(isListType);
                case nameof(Categoria):
                    return InputArgumentFor<CategoriaUpdateInput>(isListType);
                case nameof(DadosBalancoPatrimonial):
                    return InputArgumentFor<DadosBalancoPatrimonialUpdateInput>(isListType);
                case nameof(DadosDRE):
                    return InputArgumentFor<DadosDREUpdateInput>(isListType);
                case nameof(DocumentoEmpresa):
                    return InputArgumentFor<DocumentoEmpresaUpdateInput>(isListType);
                case nameof(Empresa):
                    return InputArgumentFor<EmpresaUpdateInput>(isListType);
                case nameof(GrupoCategoria):
                    return InputArgumentFor<GrupoCategoriaUpdateInput>(isListType);
                case nameof(GrupoUsuario):
                    return InputArgumentFor<GrupoUsuarioUpdateInput>(isListType);
                case nameof(Pergunta):
                    return InputArgumentFor<PerguntaUpdateInput>(isListType);
                case nameof(Pessoa):
                    return InputArgumentFor<PessoaCreateInput>(isListType);
                case nameof(Resposta):
                    return InputArgumentFor<RespostaUpdateInput>(isListType);
                case nameof(Resultado):
                    return InputArgumentFor<ResultadoUpdateInput>(isListType);
                case nameof(Socio):
                    return InputArgumentFor<SocioCreateInput>(isListType);
                case nameof(TipoContato):
                    return InputArgumentFor<TipoContatoUpdateInput>(isListType);
                case nameof(TipoDocumento):
                    return InputArgumentFor<TipoDocumentoUpdateInput>(isListType);
                case nameof(TipoExigencia):
                    return InputArgumentFor<TipoExigenciaUpdateInput>(isListType);
                case nameof(Usuario):
                    return InputArgumentFor<UsuarioUpdateInput>(isListType);
                case nameof(ValidadeDocumentoEstado):
                    return InputArgumentFor<ValidadeDocumentoEstadoUpdate>(isListType);
                case nameof(VersaoMec):
                    return InputArgumentFor<VersaoMecUpdateInput>(isListType);
                case nameof(Procuracao):
                    return InputArgumentFor<ProcuracaoUpdateInput>(isListType);
                case nameof(TermosAceite):
                    return InputArgumentFor<TermosAceiteUpdateInput>(isListType);
                case nameof(TermoAceiteEmpresa):
                    return InputArgumentFor<TermoAceiteEmpresaUpdateInput>(isListType);
                case nameof(Comentario):
                    return InputArgumentFor<ComentarioCreateInput>(isListType);
                case nameof(GrupoPerguntaQualificacao):
                    return InputArgumentFor<GrupoPerguntaQualificacaoUpdateInput>(isListType);
                case nameof(PerguntaQualificacao):
                    return InputArgumentFor<PerguntaQualificacaoUpdateInput>(isListType);
                default: return null;
            }
        }

        private static QueryArgument GetCreateInputArgument(string name, bool isListType = false)
        {
            switch (name)
            {
                case nameof(ArquivoProcessamentoPergunta):
                    return InputArgumentFor<ArquivoProcessamentoPerguntaInput>(isListType);
                case nameof(Categoria):
                    return InputArgumentFor<CategoriaCreateInput>(isListType);
                case nameof(Contato):
                    return InputArgumentFor<ContatoCreateInput>(isListType);
                case nameof(DadosBalancoPatrimonial):
                    return InputArgumentFor<DadosBalancoPatrimonialCreateInput>(isListType);
                case nameof(DadosDRE):
                    return InputArgumentFor<DadosDRECreateInput>(isListType);
                case nameof(DadosPessoaFisica):
                    return InputArgumentFor<DadosPessoaFisicaCreateInput>(isListType);
                case nameof(DocumentoEmpresa):
                    return InputArgumentFor<DocumentoEmpresaCreateInput>(isListType);
                case nameof(Empresa):
                    return InputArgumentFor<EmpresaCreateInput>(isListType);
                case nameof(Endereco):
                    return InputArgumentFor<EnderecoCreateInput>(isListType);
                case nameof(GrupoCategoria):
                    return InputArgumentFor<GrupoCategoriaCreateInput>(isListType);
                case nameof(GrupoUsuario):
                    return InputArgumentFor<GrupoUsuarioCreateInput>(isListType);
                case nameof(Pessoa):
                    return InputArgumentFor<PessoaCreateInput>(isListType);
                case nameof(Pergunta):
                    return InputArgumentFor<PerguntaCreateInput>(isListType);
                case nameof(PerguntaGrupoUsuario):
                    return InputArgumentFor<PerguntaGrupoUsuarioCreateInput>(isListType);
                case nameof(Resposta):
                    return InputArgumentFor<RespostaCreateInput>(isListType);
                case nameof(Socio):
                    return InputArgumentFor<SocioCreateInput>(isListType);
                case nameof(TipoContato):
                    return InputArgumentFor<TipoContatoCreateInput>(isListType);
                case nameof(TipoDocumento):
                    return InputArgumentFor<TipoDocumentoCreateInput>(isListType);
                case nameof(TipoExigencia):
                    return InputArgumentFor<TipoExigenciaCreateInput>(isListType);
                case nameof(Usuario):
                    return InputArgumentFor<UsuarioCreateInput>(isListType);
                case nameof(ValidadeDocumentoEstado):
                    return InputArgumentFor<ValidadeDocumentoEstadoInput>(isListType);
                case nameof(VersaoMec):
                    return InputArgumentFor<VersaoMecCreateInput>(isListType);
                case nameof(Procuracao):
                    return InputArgumentFor<ProcuracaoCreateInput>(isListType);
                case nameof(TermosAceite):
                    return InputArgumentFor<TermosAceiteCreateInput>(isListType);
                case nameof(TermoAceiteEmpresa):
                    return InputArgumentFor<TermoAceiteEmpresaCreateInput>(isListType);
                case nameof(GrupoPerguntaQualificacao):
                    return InputArgumentFor<GrupoPerguntaQualificacaoCreateInput>(isListType);
                case nameof(Comentario):
                    return InputArgumentFor<ComentarioCreateInput>(isListType);
                case nameof(PerguntaQualificacao):
                    return InputArgumentFor<PerguntaQualificacaoCreateInput>(isListType);
                case nameof(ExigenciaGrupoQualificacao):
                    return InputArgumentFor<ExigenciaGrupoQualificacaoCreateInput>(isListType);
                default: return null;
                
            }
        }

        private static QueryArgument InputArgumentFor<TType>(bool isListType) where TType: IGraphType
        {
            if(isListType)
            {
                return new QueryArgument<ListGraphType<TType>>() { Name = "input" };
            }
            return new QueryArgument<TType>() { Name = "input" };
        }
    }
}
