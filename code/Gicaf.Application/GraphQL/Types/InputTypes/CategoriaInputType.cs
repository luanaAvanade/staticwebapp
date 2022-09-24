using Gicaf.Domain.Entities;
using Gicaf.Domain.Entities.Fornecedores;
using GraphQL.Types;
using System;
using System.Collections.Generic;
using System.Text;
using Gicaf.Domain.Entities.Enums;

namespace Gicaf.Application.GraphQL.Types.InputTypes
{
    public class GrupoCategoriaCreateInput : BaseInputType<GrupoCategoria>
    {
        public GrupoCategoriaCreateInput()
        {
            Field(x => x.Nome);
            Field(x => x.Codigo);
        }
    }

    public class GrupoCategoriaUpdateInput : BaseInputType<GrupoCategoria>
    {
        public GrupoCategoriaUpdateInput()
        {
            Field(x => x.Nome, true);
            Field(x => x.Codigo, true);
        }
    }

    public class CategoriaCreateInput : BaseInputType<Categoria>
    {
        public CategoriaCreateInput()
        {
            Field(x => x.Codigo);
            Field(x => x.Descricao);
            Field(x => x.Grupos, true, typeof(ListGraphType<GrupoCategoriaCreateInput>));
        }
    }

    public class CategoriaUpdateInput : BaseInputType<Categoria>
    {
        public CategoriaUpdateInput()
        {
            Field(x => x.Codigo, true);
            Field(x => x.Descricao, true);
            Field(x => x.Grupos, true, typeof(ListGraphType<GrupoCategoriaCreateInput>));
        }
    }

    //public class CategoriaGrupoCategoriaInput : BaseInputType<CategoriaGrupoCategoria>
    //{
    //    public CategoriaGrupoCategoriaInput()
    //    {
    //        Field(x => x.GrupoCategoriaId);
    //    }
    //}

    public class PerguntaCreateInput : BaseInputType<Pergunta>
    {
        public PerguntaCreateInput()
        {
            Field(typeof(IntGraphType), nameof(Pergunta.Codigo));
            Field(x => x.Nome);
            Field(x => x.Descricao, true);
            var enumType = typeof(EnumerationGraphType<>);
            var graphTypeOrigemDados = enumType.MakeGenericType(typeof(OrigemDados));
            Field(x => x.OrigemDados, true, graphTypeOrigemDados);
            Field(x => x.PerguntaGrupoUsuario, true, typeof(ListGraphType<PerguntaGrupoUsuarioInput>));
        }
    }

    public class PerguntaUpdateInput : BaseInputType<Pergunta>
    {
        public PerguntaUpdateInput()
        {
            Field(x => x.Nome, true);
            Field(x => x.Descricao, true);
            var enumType = typeof(EnumerationGraphType<>);
            var graphTypeOrigemDados = enumType.MakeGenericType(typeof(OrigemDados));
            Field(x => x.OrigemDados, true, graphTypeOrigemDados);
            Field(x => x.PerguntaGrupoUsuario, true, typeof(ListGraphType<PerguntaGrupoUsuarioInput>));
        }
    }

     public class PerguntaGrupoUsuarioInput : BaseInputType<PerguntaGrupoUsuario>
    {
        public PerguntaGrupoUsuarioInput()
        {
            Field(x => x.PerguntaId, true);
            Field(x => x.GrupoUsuarioId);
            Field(x => x.Tipo,true , typeof(EnumerationGraphType<TipoItem>));
        }
    }
    

    public class ArquivoProcessamentoPerguntaInput : BaseInputType<ArquivoProcessamentoPergunta>
    {
        public ArquivoProcessamentoPerguntaInput()
        {
            Field(x => x.PerguntaId, true);
            //Field(typeof(string),"FileKey");

            //var enumType = typeof(EnumerationGraphType<>);
            //var graphType = enumType.MakeGenericType(typeof(TipoItem));

            Field(x => x.Tipo, true, typeof(EnumerationGraphType<TipoItem>));
            Field(x => x.Arquivo, true, typeof(ArquivoInput));
        }
    }

    public class ArquivoInput : BaseInputType<Arquivo>
    {
        public ArquivoInput()
        {
            Field(x => x.Key, true);
        }
    }

    public class VersaoMecCreateInput : BaseInputType<VersaoMec>
    {
        public VersaoMecCreateInput()
        {
            Field(x => x.Nome);
            Field(x => x.FormulaEixoX);
            Field(x => x.FormulaEixoY);
            Field(x => x.LinkMatriz);
        }
    }

    public class VersaoMecUpdateInput : BaseInputType<VersaoMec>
    {
        public VersaoMecUpdateInput()
        {
            Field(x => x.Nome, true);
            Field(x => x.FormulaEixoX, true);
            Field(x => x.FormulaEixoY, true);
            Field(x => x.DataEncerramento, true);
            Field(x => x.LinkMatriz, true);
        }
    }

    public class RespostaCreateInput : BaseInputType<Resposta>
    {
        public RespostaCreateInput()
        {
            Field(x => x.Nota);
            Field(x => x.CategoriaId);
            Field(x => x.PerguntaId);
            Field(x => x.UsuarioId);
        }
    }

    public class RespostaUpdateInput : BaseInputType<Resposta>
    {
        public RespostaUpdateInput()
        {
            Field(x => x.Nota, true);
            Field(x => x.NaoAplicavel, true);
        }
    }

    public class ResultadoUpdateInput : BaseInputType<Resultado>
    {
        public ResultadoUpdateInput()
        {
            Field(x => x.Nota, true);
        }
    }

    public class GerarRespostasInput : BaseInputType<Resposta>
    {
        public GerarRespostasInput()
        {
            Field(x => x.PerguntaId);
            Field(x => x.UsuarioId);
        }
    }

    public class UsuarioAssociateInput : BaseInputType<Usuario>
    {
        public UsuarioAssociateInput()
        {
            Field(x => x.Id);
        }
    }

    public class GrupoUsuarioCreateInput : BaseInputType<GrupoUsuario>
    {
        public GrupoUsuarioCreateInput()
        {
            Field(x => x.Nome);
            Field(x => x.Descricao,true);
            Field(x => x.Usuarios, true, typeof(ListGraphType<UsuarioAssociateInput>));
        }
    }

    public class GrupoUsuarioUpdateInput : BaseInputType<GrupoUsuario>
    {
        public GrupoUsuarioUpdateInput()
        {
            Field(x => x.Nome,true);
            Field(x => x.Descricao,true);
            Field(x => x.Usuarios, true, typeof(ListGraphType<UsuarioAssociateInput>));
        }
    }

    public class UsuarioUpdateInput : BaseInputType<Usuario>
    {
        public UsuarioUpdateInput()
        {
            Field(x => x.Id, true);
            Field(x => x.Nome, true);
            Field(x => x.GrupoUsuarioId, true);
        }
    }

    public class PerguntaGrupoUsuarioCreateInput : BaseInputType<PerguntaGrupoUsuario>
    {
        public PerguntaGrupoUsuarioCreateInput()
        {
            Field(x => x.PerguntaId);
            Field(x => x.GrupoUsuarioId);
            Field(x => x.Tipo,false , typeof(EnumerationGraphType<TipoItem>));
        }
    }

    public class TipoContatoCreateInput : BaseInputType<TipoContato>
    {
        public TipoContatoCreateInput()
        {
            Field(x => x.Nome);
            Field(x => x.Descricao);
            Field(x => x.Status);
        }
    }

    public class TipoContatoUpdateInput : BaseInputType<TipoContato>
    {
        public TipoContatoUpdateInput()
        {
            Field(x => x.Nome, true);
            Field(x => x.Descricao, true);
            Field(x => x.Status,true);
        }
    }

    public class TipoDocumentoCreateInput : BaseInputType<TipoDocumento>
    {
        public TipoDocumentoCreateInput()
        {
            Field(x => x.Nome);
            Field(x => x.Help);
            Field(x => x.Obrigatorio);
            Field(x => x.QuantidadeMaxima, true); 
            Field(x => x.TamanhoMaximo, true);
            Field(x => x.ValidadeMeses, true);
            Field(x => x.Status, true);
            Field(x => x.TiposArquivos, true, typeof(IntGraphType));
            Field(x => x.TipoDocumentoFuncionalidade,false, typeof(ListGraphType<TipoDocumentoFuncionalidadeCreateInput>));
        }
    }

    public class TipoDocumentoUpdateInput : BaseInputType<TipoDocumento>
    {
        public TipoDocumentoUpdateInput()
        {
            Field(x => x.Nome, true);
            Field(x => x.Help, true);
            Field(x => x.Obrigatorio, true);   
            Field(x => x.QuantidadeMaxima, true); 
            Field(x => x.TamanhoMaximo, true);
            Field(x => x.Status, true);
            Field(x => x.ValidadeMeses, true);
            Field(x => x.TiposArquivos, true, typeof(IntGraphType));
            Field(x => x.TipoDocumentoFuncionalidade, true, typeof(ListGraphType<TipoDocumentoFuncionalidadeUpdateInput>));
                                                                                                
        }
    }

    public class TipoExigenciaCreateInput : BaseInputType<TipoExigencia>
    {
        public TipoExigenciaCreateInput()
        {
            Field(x => x.Nome);
            Field(x => x.Descricao);
           Field(x => x.NivelExigencia, false, typeof(EnumerationGraphType<NivelTipoExigencia>));
            Field(x => x.Status);
        }
    }

    public class TipoExigenciaUpdateInput : BaseInputType<TipoExigencia>
    {
        public TipoExigenciaUpdateInput()
        {
            Field(x => x.Nome, true);
            Field(x => x.Descricao, true);
           Field(x => x.NivelExigencia, false, typeof(EnumerationGraphType<NivelTipoExigencia>));
            Field(x => x.Status);
        }
    }

    public class TipoDocumentoFuncionalidadeCreateInput : BaseInputType<TipoDocumentoFuncionalidade>
    {
        public TipoDocumentoFuncionalidadeCreateInput()
        {
            Field(x => x.Id, true);
            Field(x => x.TipoDocumentoId, true);
            Field(x => x.Funcionalidade, false, typeof(EnumerationGraphType<Funcionalidades>));
            Field(x => x.Obrigatorio);
            Field(x => x.ValidadeMeses, true);
            Field(x => x.ValidadeDocumentoEstado, true, typeof(ListGraphType<ValidadeDocumentoEstadoInput>));
        }
    }

    public class TipoDocumentoFuncionalidadeUpdateInput : BaseInputType<TipoDocumentoFuncionalidade>
    {
        public TipoDocumentoFuncionalidadeUpdateInput()
        {
            Field(x => x.Id, true);
            Field(x => x.TipoDocumentoId, true);
            Field(x => x.Funcionalidade, true, typeof(EnumerationGraphType<Funcionalidades>));
            Field(x => x.Obrigatorio, true);
            Field(x => x.ValidadeMeses, true);
            Field(x => x.ValidadeDocumentoEstado, true, typeof(ListGraphType<ValidadeDocumentoEstadoInput>));
        }
    }

    public class ValidadeDocumentoEstadoInput : BaseInputType<ValidadeDocumentoEstado>
    {
        public ValidadeDocumentoEstadoInput()
        {
            Field(x => x.Id, true);
            Field(x => x.TipoDocumentoFuncionalidadeId, true);
            Field(x => x.EstadoId);
            Field(x => x.ValidadeMeses, true);
            Field(x => x.Obrigatorio,true);
        }
    }

    public class ValidadeDocumentoEstadoUpdate : BaseInputType<ValidadeDocumentoEstado>
    {
        public ValidadeDocumentoEstadoUpdate()
        {
            Field(x => x.Id, true);
            Field(x => x.TipoDocumentoFuncionalidadeId, true);
            Field(x => x.EstadoId, true);
            Field(x => x.ValidadeMeses, true);
            Field(x => x.Obrigatorio,true);
        }
    }

    public class PessoaCreateInput : BaseInputType<Pessoa>
    {
        public PessoaCreateInput()
        {
            Field(x => x.Nome);
            Field(x => x.CPF);
            Field(x => x.Telefone);
            Field(x => x.Email);
            Field(x => x.Celular);
        }
    }

    public class SocioCreateInput : BaseInputType<Socio>
    {
        public SocioCreateInput()
        {
            Field(x => x.EmpresaFornecedoraId, true);
            Field(x => x.ValorParticipacao, true);
            Field(x => x.TipoPessoa, false, typeof(EnumerationGraphType<TipoPessoa>));
            Field(x => x.Codigo);
            Field(x => x.Nome);
            Field(x => x.TipoSocio, false, typeof(EnumerationGraphType<TipoSocio>));
            Field(x => x.Administrador, true);
            Field(x => x.Procuracoes, true, typeof(ListGraphType<ProcuracaoCreateInput>));
        
        }
    }

    public class SocioUpdateInput : BaseInputType<Socio>
    {
        public SocioUpdateInput()
        {
            Field(x => x.EmpresaFornecedoraId, true);
            Field(x => x.ValorParticipacao, true);
            Field(x => x.TipoPessoa, false, typeof(EnumerationGraphType<TipoPessoa>));
            Field(x => x.Nome, true);
            Field(x => x.Codigo, true);
            Field(x => x.TipoSocio, true, typeof(EnumerationGraphType<TipoSocio>));
            Field(x => x.Administrador, true);
            Field(x => x.Id, true);
            Field(x => x.Procuracoes, true, typeof(ListGraphType<ProcuracaoUpdateInput>));
        }
    }

public class ProcuracaoCreateInput : BaseInputType<Procuracao>
    {
        public ProcuracaoCreateInput()
        {
            Field(x => x.SocioId, true);
            Field(x => x.OutorganteId, true);
            Field(x => x.Validade, true);        
        }
    }

    public class ProcuracaoUpdateInput : BaseInputType<Procuracao>
    {
        public ProcuracaoUpdateInput()
        {
            
            Field(x => x.SocioId, true);
            Field(x => x.OutorganteId, true);
            Field(x => x.Validade, true);
            Field(x => x.Id, true);
        }
    }

public class GrupoDeAssinaturaCreateInput : BaseInputType<GrupoDeAssinatura>
    {
        public GrupoDeAssinaturaCreateInput()
        {
            Field(x => x.EmpresaFornecedoraId, true);
            Field(x => x.TipoAssinatura, false, typeof(EnumerationGraphType<TipoAssinatura>));
            Field(x => x.ValorLimite);
            Field(x => x.Assinaturas, true, typeof(ListGraphType<AssinaturaSocioCreateInput>));
        }
    }

    public class GrupoDeAssinaturaUpdateInput : BaseInputType<GrupoDeAssinatura>
    {
        public GrupoDeAssinaturaUpdateInput()
        {
            Field(x => x.EmpresaFornecedoraId, true);
            Field(x => x.TipoAssinatura, false, typeof(EnumerationGraphType<TipoAssinatura>));
            Field(x => x.ValorLimite);
            Field(x => x.Assinaturas, true, typeof(ListGraphType<AssinaturaSocioUpdateInput>));
            Field(x => x.Id, true);
        }
    }
    
public class AssinaturaSocioCreateInput : BaseInputType<AssinaturaSocio>
    {
        public AssinaturaSocioCreateInput()
        {
            Field(x => x.GrupoDeAssinaturaId, true);
            Field(x => x.SocioId);
            Field(x => x.Obrigatoriedade);
        }
    }

    public class AssinaturaSocioUpdateInput : BaseInputType<AssinaturaSocio>
    {
        public AssinaturaSocioUpdateInput()
        {
            Field(x => x.GrupoDeAssinaturaId, true);
            Field(x => x.SocioId);
            Field(x => x.Obrigatoriedade);
            Field(x => x.Id, true);
        }
    }

    public class DocumentoEmpresaCreateInput : BaseInputType<DocumentoEmpresa>
    {
        public DocumentoEmpresaCreateInput()
        {
            Field(x => x.EmpresaId, true);
            Field(x => x.DataBasePeriodo, true);
            Field(x => x.TipoDocumentoId);    
            Field(x => x.Arquivo, false, typeof(ArquivoInput));
        }
    }

    public class DocumentoEmpresaUpdateInput : BaseInputType<DocumentoEmpresa>
    {
        public DocumentoEmpresaUpdateInput()
        {
            Field(x => x.EmpresaId);
            Field(x => x.ArquivoId);
            Field(x => x.DataBasePeriodo);
            Field(x => x.TipoDocumentoId);    
            Field(x => x.Arquivo, false, typeof(ArquivoInput));
        }
    }

 public class TermosAceiteCreateInput : BaseInputType<TermosAceite>
    {
        public TermosAceiteCreateInput()
        {
            Field(x => x.Titulo);
            Field(x => x.SubTitulo);
            Field(x => x.Status);    
            Field(x => x.TipoFornecedor);
            Field(x => x.TipoCadastro);
            Field(x => x.Descricao);  
            Field(x => x.TermoAceiteEmpresa, true, typeof(ListGraphType<TermoAceiteEmpresaCreateInput>));
        }
    }

    public class TermosAceiteUpdateInput : BaseInputType<TermosAceite>
    {
        public TermosAceiteUpdateInput()
        {
             Field(x => x.Titulo);
            Field(x => x.SubTitulo);
            Field(x => x.Status);    
            Field(x => x.TipoFornecedor);
            Field(x => x.TipoCadastro);
            Field(x => x.Descricao);  
        }
    }

    public class TermoAceiteEmpresaCreateInput : BaseInputType<TermoAceiteEmpresa>
    {
        public TermoAceiteEmpresaCreateInput()
        {
            Field(x => x.Aceite );
            Field(x => x.EmpresaId);    
            Field(x => x.TermosAceiteId);
        }
    }

    public class TermoAceiteEmpresaUpdateInput : BaseInputType<TermoAceiteEmpresa>
    {
        public TermoAceiteEmpresaUpdateInput()
        {   
            Field(x => x.Id, true);
            Field(x => x.Aceite, true );
            Field(x => x.EmpresaId, true);    
            Field(x => x.TermosAceiteId, true);
        }
    }

    public class AnaliseCadastroCreateInput : BaseInputType<AnaliseCadastro>
    {
        public AnaliseCadastroCreateInput()
        {
            Field(x => x.AtribuidoId, true);
            Field(x => x.EmpresaFornecedoraId, true);
            Field(x => x.ItensAnalise, true, typeof(ListGraphType<ItemAnaliseCreateInput>));
            Field(x => x.StatusAnalise, true, typeof(EnumerationGraphType<StatusAnalise>));        
            Field(x => x.TransmitidoId, true);
        }
    }

    public class AnaliseCadastroUpdateInput : BaseInputType<AnaliseCadastro>
    {
        public AnaliseCadastroUpdateInput()
        {
            Field(x => x.Id, true);
            Field(x => x.AtribuidoId, true);
            Field(x => x.EmpresaFornecedoraId, true);
            Field(x => x.ItensAnalise, true, typeof(ListGraphType<ItemAnaliseUpdateInput>));
            Field(x => x.StatusAnalise, true, typeof(EnumerationGraphType<StatusAnalise>));        
            Field(x => x.TransmitidoId, true);
        }
    }

    public class ItemAnaliseCreateInput : BaseInputType<ItemAnalise>
    {
        public ItemAnaliseCreateInput()
        {
            Field(x => x.AnaliseId);    
            Field(x => x.TipoItem, false, typeof(EnumerationGraphType<EnumItemAnalise>));
            Field(x => x.AutorId, true);
            Field(x => x.Status, false, typeof(EnumerationGraphType<StatusAnalise>));   
            Field(x => x.ArquivoId, true); 
            Field(x => x.Justificativa, true);
        }
    }
    public class ItemAnaliseUpdateInput : BaseInputType<ItemAnalise>
    {
        public ItemAnaliseUpdateInput()
        {
            Field(x => x.Id, true);
            Field(x => x.AnaliseId, true);    
            Field(x => x.TipoItem, true, typeof(EnumerationGraphType<EnumItemAnalise>));
            Field(x => x.AutorId, true);
            Field(x => x.Status, true, typeof(EnumerationGraphType<StatusAnalise>));  
            Field(x => x.ArquivoId, true);   
            Field(x => x.Justificativa, true); 
        }
    }
    
     public class GrupoPerguntaQualificacaoCreateInput : BaseInputType<GrupoPerguntaQualificacao>
    {
        public GrupoPerguntaQualificacaoCreateInput()
        {
            Field(x => x.Nome);
            Field(x => x.Status);
        }
    }

    public class GrupoPerguntaQualificacaoUpdateInput : BaseInputType<GrupoPerguntaQualificacao>
    {
        public GrupoPerguntaQualificacaoUpdateInput()
        {
            Field(x => x.Nome, true);
            Field(x => x.Status, true);
        }
    }

    public class PerguntaQualificacaoCreateInput : BaseInputType<PerguntaQualificacao>
    {
        public PerguntaQualificacaoCreateInput()
        {
            Field(x => x.GrupoPerguntaQualificacaoId,true);
            Field(x => x.Texto,true);
            Field(x => x.TipoResposta,true, typeof(EnumerationGraphType<TipoResposta>));
            Field(x => x.ParametroResposta,true);
            Field(x => x.Dica);
            Field(x => x.Validade, true);
            Field(x => x.QuemResponde, false, typeof(EnumerationGraphType<Perfil>) );
            Field(x => x.QuemVisualiza,true, typeof(EnumerationGraphType<Perfil>) );
            Field(x => x.Obrigatorio);
            Field(x => x.PossuiAnexo);
            Field(x => x.TamanhoMaximoArquivo,true);
            Field(x => x.Status);
        }
    }

    public class PerguntaQualificacaoUpdateInput : BaseInputType<PerguntaQualificacao>
    {
        public PerguntaQualificacaoUpdateInput()
        {
            Field(x => x.GrupoPerguntaQualificacaoId,true);
            Field(x => x.Texto,true);
            Field(x => x.TipoResposta,true, typeof(EnumerationGraphType<TipoResposta>));
            Field(x => x.ParametroResposta,true);
            Field(x => x.Dica,true);
            Field(x => x.Validade,true);
            Field(x => x.QuemResponde, true, typeof(EnumerationGraphType<Perfil>) );
            Field(x => x.QuemVisualiza,true, typeof(EnumerationGraphType<Perfil>) );
            Field(x => x.Obrigatorio,true);
            Field(x => x.PossuiAnexo,true);
            Field(x => x.TamanhoMaximoArquivo,true);
            Field(x => x.Status,true);
        }
    }
    
}